
using Domain.TicTacToe.GameEvents;

namespace Domain.TicTacToe;
public interface ITicTacToeGameEngine
{
    public Task<IEnumerable<TicTacToeGameEvent>> MakeTurnAsync(TicTacToeGame game, int row, int column);

    public Task<IEnumerable<TicTacToeGameEvent>> ExitRoomAsync(TicTacToeGameRoom room, string userId);

    public Task<IEnumerable<TicTacToeGameEvent>> JoinRoomAsync(TicTacToeGameRoom room, string userId);
}


public interface ITicTacToeGameEngineRunner
{
    public Task MakeTurnAsync(string roomId, string userId, int row, int column);

    public Task ExitGameAsync(string roomId, string userId);

    public Task JoinRoomAsync(string roomId, string userId);
}
