using Domain.TicTacToe;

namespace DataAccess;

public interface IGameRoomRepository
{
    public Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(string sessionId); 

    public Task<TicTacToeGameRoom?> GetGameSessionByIdAsync(string sessionId);

    public Task UpdateSessionStatusAsync(string sessionId, TicTacToeRoomState state);

    public Task UpdateRoomGameAsync(string roomId, TicTacToeGame game);

    public Task UpdateSessionAsync(TicTacToeGameRoom session);
}

public class GameRoomRepository : IGameRoomRepository
{
    public Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task<TicTacToeGameRoom?> GetGameSessionByIdAsync(string sessionId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRoomGameAsync(string roomId, TicTacToeGame game)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSessionAsync(TicTacToeGameRoom session)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSessionStatusAsync(string sessionId, TicTacToeRoomState state)
    {
        throw new NotImplementedException();
    }
}
