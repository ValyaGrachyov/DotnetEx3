using Domain.TicTacToe;

namespace DataAccess;

public interface IGameRoomRepository
{
    public Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(Guid sessionId); 

    public Task UpdateSessionStatusAsync(Guid sessionId, TicTacToeRoomState state);

    public Task UpdateRoomGameAsync(Guid roomId, TicTacToeGame game);

    public Task UpdateSessionAsync(TicTacToeGameRoom session);
}