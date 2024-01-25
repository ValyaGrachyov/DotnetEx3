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

namespace TicacToe_Backend.Helpers.Extensions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicTacToeContext>(options =>
        {
            options.UseNpgsql(configuration.GetSection("DB:Postgres:ConnectionString").Value);
        });
        return services;
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new MongoClient(configuration.GetSection("DB:Mongo:ConnectionString").Value));
        return services;
    }

    public static IServiceCollection AddMetdiator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
        return services;
    }

    public static IServiceCollection AddIdentityWithJWT(this IServiceCollection services)
    {
        services.AddTransient<IJwtGenerator, JwtGenerator>();
        
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<TicTacToeContext>()
            .AddDefaultTokenProviders();
        
        return services;
    }
}