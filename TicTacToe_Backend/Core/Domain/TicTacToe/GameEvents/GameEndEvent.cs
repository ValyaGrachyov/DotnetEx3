namespace Domain.TicTacToe.GameEvents;

public class GameEndEvent : TicTacToeGameEvent
{
    public string? WinnerId { get; set; }

    public string? WinnerName { get; set; }
}

