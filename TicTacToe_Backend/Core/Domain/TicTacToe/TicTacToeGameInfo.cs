namespace Domain.TicTacToe;

public class TicTacToeGameInfo
{
    public TicTacToeSymbols[] GameField { get; init; } = new TicTacToeSymbols[]
    {
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None,
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None,
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None
    };

    public bool IsPlayer1Turn { get; set; }

    public TicTacToePlayer Player1 { get; init; }

    public TicTacToePlayer Player2 { get; init; }

    public DateTime? LastGameActionTimeUtc { get; set; }
}
