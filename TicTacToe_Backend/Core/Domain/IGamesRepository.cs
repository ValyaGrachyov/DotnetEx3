using Domain.TicTacToe;

namespace DataAccess;

public interface IGameRoomRepository
{
    public Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(Guid sessionId);

    public Task<IEnumerable<TicTacToeGameRoom>?> GetGameRooms();

    public Task UpdateSessionStatusAsync(Guid sessionId, TicTacToeRoomState state);

    public Task UpdateRoomGameAsync(Guid roomId, TicTacToeGame game);

    public Task UpdateSessionAsync(TicTacToeGameRoom session);

    public Task<string> CreateRoom(int maxRate, string creatorId, string creatorUserName);
}