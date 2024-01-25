namespace Domain.TicTacToe.GameEvents;

public class NewGameStartEvent : TicTacToeGameEvent
{
    public string GameSessionId { get; init; }

    public TicTacToePlayer Player1 { get; init; }

    public TicTacToePlayer Player2 { get; init; }

    public bool IsPlayer1Turn { get; init; }
}
