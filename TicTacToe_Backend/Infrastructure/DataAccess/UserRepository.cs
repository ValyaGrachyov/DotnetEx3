using Migrations;

namespace DataAccess;

public class UserRepository : IUserRepository
{
    private readonly TicTacToeContext _ctx;


    public UserRepository(TicTacToeContext _ctx)
    {

    }

    public Task<string> GetUserByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}