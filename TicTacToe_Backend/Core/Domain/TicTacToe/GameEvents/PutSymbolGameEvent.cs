namespace Domain.TicTacToe.GameEvents;

public class PutSymbolGameEvent : UserActionGameEvent
{

    public int Row { get; init; }

    public int Column { get; init; }

    public TicTacToeSymbols PutSymbol { get; init; } 
}
