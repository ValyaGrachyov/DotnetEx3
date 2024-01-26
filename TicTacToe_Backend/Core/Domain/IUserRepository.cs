

namespace DataAccess;

public interface IUserRepository
{
    public Task<string> GetUserByIdAsync(string userId);
}

