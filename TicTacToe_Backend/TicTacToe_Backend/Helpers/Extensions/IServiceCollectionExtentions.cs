using System.Reflection;
using System.Reflection.Metadata;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Migrations;
using Features;
using MongoDB.Driver;
using MediatR;
using Microsoft.AspNetCore.Identity;
using TicacToe_Backend.Helpers.Authorization;
using AssemblyReference = Features.AssemblyReference;
using DataAccess.ServiceRegistration;
using Domain.TicTacToe;
using TicacToe_Backend.InfrastructureService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Features.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace TicacToe_Backend.Helpers.Extensions;

public static class ServiceCollectionExtentions
{
    private static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicTacToeContext>(options =>
        {
            options.UseNpgsql(configuration.GetSection("DB:Postgres:ConnectionString").Value);
        });
        return services;
    }

    private static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(configuration.GetSection("DB:Mongo:ConnectionString").Value));
        return services;
    }

    public static IServiceCollection AddMetdiator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
        return services;
    }

    private static IServiceCollection AddIdentityWithJWT(this IServiceCollection services)
    {
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<TicTacToeContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPostgres(configuration)
            .AddMongo(configuration);

        return services.AddUserRepository()
            .AddGamesRepository(configuration);
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITicTacToeGameEngine, GameEngine>();
        services.AddScoped<IUpdateRecorder, GameEventBroadcaster>();

        services.AddMassTransit(x =>
        {
            x.AddConsumers(Features.AssemblyReference.Assembly);

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(configuration
                        .GetSection(RabbitMqConfig.SectionName)
                        .Get<RabbitMqConfig>()!
                        .FullHostname);
                cfg.ConfigureEndpoints(ctx);
            });
        });

        services.AddDataAccess(configuration);
        services.AddScoped<IAwarder, RateUpdatePublisher>();
        return services;
    }

    public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityWithJWT();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.SaveToken = true;

            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(configuration["JWT:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                RequireExpirationTime = false
            };
        });

        services.AddAuthorization();

        return services;
    }
}