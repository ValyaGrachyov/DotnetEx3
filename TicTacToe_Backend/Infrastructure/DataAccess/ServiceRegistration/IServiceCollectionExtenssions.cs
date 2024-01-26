using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.ServiceRegistration;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddUserRepository(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }

    public static IServiceCollection AddGamesRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GamesCollectionParams>(configuration.GetSection("DB:Mongo"));
        services.AddScoped<IGameRoomRepository, GameRoomRepository>();

        return services;
    }
}
