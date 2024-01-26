namespace Domain.TicTacToe.Exceptions;

public class AccessDeniedGameException : TicTacToeGameException
{ 
    public AccessDeniedGameException() : base("You are not participant of current game.") { }
}
