
namespace Domain.TicTacToe.GameEvents;
public class RoomWasClosedGameEvent : TicTacToeGameEvent
{
    public Guid RoomId { get; set; }
}
