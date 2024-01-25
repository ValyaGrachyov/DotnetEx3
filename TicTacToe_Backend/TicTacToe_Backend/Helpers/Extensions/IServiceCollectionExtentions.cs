using Microsoft.EntityFrameworkCore;
using Migrations;
using MongoDB.Driver;

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
}