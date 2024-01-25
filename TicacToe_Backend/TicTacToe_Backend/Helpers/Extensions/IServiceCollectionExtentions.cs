using Microsoft.EntityFrameworkCore;
using Migrations;

namespace TicacToe_Backend.Helpers.Extensions;

public static class ServiceCollectionExtentions
{
    public static IServiceCollection AddPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TicTacToeContext>(options =>
        {
            options.UseNpgsql(configuration.GetSection("DB")
                                            .GetSection("Postgres")
                                            .GetConnectionString("ConnectionString"));
        });
        return services;
    }
}