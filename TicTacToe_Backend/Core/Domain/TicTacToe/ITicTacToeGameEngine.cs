
using Domain.TicTacToe.GameEvents;

namespace Domain.TicTacToe;
public interface ITicTacToeGameEngine
{
    public Task<IEnumerable<TicTacToeGameEvent>> RestartGameAsync(TicTacToeGameRoom room);

    public Task<IEnumerable<TicTacToeGameEvent>> MakeTurnAsync(TicTacToeGame game, int row, int column);

    public Task<IEnumerable<TicTacToeGameEvent>> ExitRoomAsync(TicTacToeGameRoom room, string userId);

    public Task<IEnumerable<TicTacToeGameEvent>> JoinRoomAsync(TicTacToeGameRoom room, string userId, string Username);
}