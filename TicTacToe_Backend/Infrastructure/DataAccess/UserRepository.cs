using Domain.TicTacToe;
using Domain.UserStatistics;
using Migrations;
using MongoDB.Driver;

namespace DataAccess;

public class UserRepository : IUserRepository
{
    private readonly TicTacToeContext _ctx;

    public UserRepository(TicTacToeContext _ctx)
    {

    }

    Task<string> IUserRepository.GetUserByIdAsync(string userId)
    {
        throw new NotImplementedException();
    }
}