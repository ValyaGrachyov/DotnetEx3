

namespace DataAccess;

public interface IUserRepository
{
    public Task<string> GetUserByIdAsync(string userId);
}

public class UserRepository : IUserRepository
{
    public Task<string> GetUserByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
