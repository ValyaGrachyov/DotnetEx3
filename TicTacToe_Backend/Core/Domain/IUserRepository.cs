

using Domain.Entities;

namespace DataAccess;

public interface IUserRepository
{
    public Task<User> GetUserByIdAsync(string userId);
    public Task UpdateuserRate(string userId, int updatedRate);
    public Task<int> GetUserRateByIdAsync(string userId);
}

