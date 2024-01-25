using Domain.TicTacToe.GameEvents;

namespace Domain.TicTacToe;

public interface IUpdateRecorder
{
    public Task RecordUpdateAsync(TicTacToeGameEvent update);
}
