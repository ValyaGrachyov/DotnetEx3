namespace Domain.TicTacToe.GameEvents;


public class UserActionGameEvent : TicTacToeGameEvent
{
    public string UserId { get; init; }


    public string Username { get; set; }
}