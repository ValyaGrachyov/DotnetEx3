using Domain.Entities;
using Domain.UserStatistics;
using Microsoft.Extensions.Options;
using Migrations;
using MongoDB.Driver;

namespace DataAccess;

public class UserRepository : IUserRepository
{
    private readonly TicTacToeContext _ctx;
    private readonly IMongoCollection<UserRate> _collection;

    public UserRepository( TicTacToeContext ctx, IMongoClient client, IOptions<RateCollectionParams> settingsOptions)
    {
        var settings = settingsOptions.Value;
        _collection = client.GetDatabase(settings.DatabaseName).GetCollection<UserRate>(settings.CollectionName);
        _ctx = ctx;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        var user = await _ctx.Users.FindAsync(userId);
        var userRate = await GetUserRateByIdAsync(userId);
        
        

        if (user != null)
        {
            var newUser = new User()
            {
                Id = user.Id,
                UserName = user.UserName,
                Rate = userRate
            };            

            return newUser;
        }
        return null;
    }
    
    
    
    public async Task<IEnumerable<UserRate>> GetUsersRate()
    {
        var filter = Builders<UserRate>.Filter.Where(x => true);

        var data = await _collection.Find(filter)
            .SortByDescending(x => x.Rate)
            .ToListAsync();

        return data;
    }

    public async Task CreateUser(string id, string username)
    {
        var newUserRate = new UserRate()
        {
            Id = new Guid(),
            UserId = id,
            Username = username,
            Rate = 0
        };

        await _collection.InsertOneAsync(newUserRate);
    }

    public Task UpdateUserRateAsync(string userId, int updatedRate)
    {
        var filter = Builders<UserRate>.Filter.Eq(x => x.UserId, userId);
        var update =  Builders<UserRate>.Update
            .Set(x => x.Rate, updatedRate);

        return  _collection.UpdateOneAsync(filter, update);
    }

    


    public async Task<int> GetUserRateByIdAsync(string userId)
    {
        var userRate = await _collection.FindAsync(x => x.UserId == userId);
        return userRate.FirstOrDefault()?.Rate ?? 0;
    }
}