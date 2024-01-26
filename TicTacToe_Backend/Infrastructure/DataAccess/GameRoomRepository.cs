using Domain.TicTacToe;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
        var cursor = await _collection.FindAsync(x => x.RoomId == sessionId);

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TicTacToeGameRoom>?> GetGameRooms()
    {
        var rooms = await _collection.FindAsync(x => true);

        return rooms.ToEnumerable();
    }
        
    

    public async Task UpdateRoomGameAsync(Guid roomId, TicTacToeGame game)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.RoomId, roomId);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrnetGame, game);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateSessionAsync(TicTacToeGameRoom session)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.RoomId, session.RoomId);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrnetGame, session.CurrnetGame)
            .Set(s => s.OpponentId, session.OpponentId)
            .Set(s => s.CurrentGameState, session.CurrentGameState);
        
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task UpdateSessionStatusAsync(Guid sessionId, TicTacToeRoomState state)
    {
        var filter = Builders<TicTacToeGameRoom>.Filter.Eq(s => s.RoomId, sessionId);
        var update = Builders<TicTacToeGameRoom>.Update
            .Set(s => s.CurrentGameState, state);

        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task<string> CreateRoom(int maxRate, string creatorId, string creatorUserName)
    {
        var gameRoom = new TicTacToeGameRoom()
        {
            RoomId = new Guid(),
            CurrentGameState = TicTacToeRoomState.WaitingForOpponent,
            RoomCreatorId = creatorId,
            CreationDateTimeUtc = DateTime.Now.Date,
            MaxAllowedPlayerRate = maxRate,
            CreatorUserName = creatorUserName,
            OpponentId = null,
            CurrnetGame = null
        };

        await _collection.InsertOneAsync(gameRoom);

        return gameRoom.RoomId.ToString();
    }
}
