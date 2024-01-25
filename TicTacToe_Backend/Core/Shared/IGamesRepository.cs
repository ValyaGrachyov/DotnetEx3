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