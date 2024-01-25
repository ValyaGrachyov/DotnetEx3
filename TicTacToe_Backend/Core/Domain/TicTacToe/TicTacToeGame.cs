namespace Domain.TicTacToe;

public class TicTacToeGame
{
    public string RoomId { get; init; }

    public TicTacToeSymbols[] GameField { get; init; } = new TicTacToeSymbols[]
    {
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None,
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None,
      TicTacToeSymbols.None, TicTacToeSymbols.None, TicTacToeSymbols.None
    };

    public bool IsPlayer1Turn { get; set; }

    public TicTacToePlayer Player1 { get; init; }

    public TicTacToePlayer Player2 { get; init; }

    public Winner? Winner { get; set; }

    public DateTime? LastGameActionTimeUtc { get; set; }
}

public enum Winner
{
    Player1,
    Player2,
    Nobody
}
