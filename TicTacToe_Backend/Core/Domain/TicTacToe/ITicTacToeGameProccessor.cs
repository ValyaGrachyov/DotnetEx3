
namespace Domain.TicTacToe;
public interface ITicTacToeGameProccessor
{
    public Task MakeTurnAsync(int row, int column);

    public Task ExitGameAsync();

    public Task JoinGameAsync();
}
