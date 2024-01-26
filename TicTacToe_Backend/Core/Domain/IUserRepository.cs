using Domain.Entities;
using Domain.UserStatistics;

namespace DataAccess;

public interface IUserRepository
{
    public Task<User> GetUserByIdAsync(string userId);

    public Task UpdateUserRateAsync(string userId, int updatedRate);

    public Task<IEnumerable<UserRate>> GetUsersRate();

    public Task CreateUser(string userId, string username);


}

