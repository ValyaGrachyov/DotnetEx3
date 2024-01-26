using Domain.TicTacToe;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;

namespace DataAccess;
public class GameRoomRepository : IGameRoomRepository
{
    private readonly IMongoCollection<TicTacToeGameRoom> _collection;

    public GameRoomRepository(IMongoClient client, IOptions<GamesCollectionParams> settingsOptions)
    {
        var settings = settingsOptions.Value;
        _collection = client.GetDatabase(settings.DatabaseName).GetCollection<TicTacToeGameRoom>(settings.CollectionName);
    }

    public async Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(Guid sessionId)
    {
        var cursor = await _collection.FindAsync(x => x.Id == sessionId);

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TicTacToeGameRoom>?> GetGameRooms(int page, int limit)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Where(x => true);

        var data = await _collection.Find(filter)
            .SortByDescending(x => x.CreationDateTimeUtc)
            .ThenByDescending(x => x.CurrentGameState)
            .Skip((page - 1) * limit)
            .Limit(limit)
            .ToListAsync();

        return data;
    }   

    public async Task UpdateRoomGameAsync(Guid roomId, TicTacToeGame game)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.Id, roomId);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrnetGame, game);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateSessionAsync(TicTacToeGameRoom session)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.Id, session.Id);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrnetGame, session.CurrnetGame)
            .Set(s => s.OpponentId, session.OpponentId)
            .Set(s => s.OpponentUserName, session.OpponentUserName)
            .Set(s => s.CurrentGameState, session.CurrentGameState);
        
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateSessionStatusAsync(Guid sessionId, TicTacToeRoomState state)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.Id, sessionId);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrentGameState, state);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<Guid> AddRoomAsync(int maxRate, string creatorId, string creatorUserName)
    {
        var gameRoom = new TicTacToeGameRoom()
        {
            Id = Guid.NewGuid(),
            CurrentGameState = TicTacToeRoomState.WaitingForOpponent,
            RoomCreatorId = creatorId,
            CreationDateTimeUtc = DateTime.UtcNow,
            MaxAllowedPlayerRate = maxRate,
            CreatorUserName = creatorUserName,
            OpponentId = null,
            CurrnetGame = null
        };

        await _collection.InsertOneAsync(gameRoom);

        return gameRoom.Id;
    }

    public Task RemoveRoomGameByIdAsync(Guid id)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(x => x.Id, id);
        return _collection.FindOneAndDeleteAsync(filter);
    }
}
