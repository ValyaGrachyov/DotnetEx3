
namespace Domain.TicTacToe;
public interface ITicTacToeGameProccessor
{
    public void MakeTurn(int row, int column);

    public void ExitGame();

    public void JoinGame();
}
