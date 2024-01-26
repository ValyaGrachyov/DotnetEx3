namespace Domain.TicTacToe.GameEvents;

public class TicTacToeGameEvent
{
    public TicTacToeGameEvent()
    {
        EventName = GetType().Name;
    }

    public Guid RoomId { get; init; }

    public string EventName { get; }
}
