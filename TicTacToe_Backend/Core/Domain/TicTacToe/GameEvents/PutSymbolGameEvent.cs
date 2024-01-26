namespace Domain.TicTacToe.GameEvents;

public class PutSymbolGameEvent : TicTacToeGameEvent
{
    public string UserId { get; init; }

    public string UserName { get; init; }

    public int Row { get; init; }

    public int Column { get; init; }

    public TicTacToeSymbols PutSymbol { get; init; } 

}
