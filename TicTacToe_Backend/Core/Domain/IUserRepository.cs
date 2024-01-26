using Domain.Entities;

namespace DataAccess;

public interface IUserRepository
{
    public Task<User> GetUserByIdAsync(string userId);

    public Task UpdateUserRateAsync(string userId, int updatedRate);
}

