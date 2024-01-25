namespace Domain.TicTacToe.GameEvents;

public class PutSymbolGameEvent : TicTacToeGameEvent
{
    public int Row { get; set; }

    public int Column { get; set; }

    public TicTacToeSymbols PutSymbol { get; set; } 
}
