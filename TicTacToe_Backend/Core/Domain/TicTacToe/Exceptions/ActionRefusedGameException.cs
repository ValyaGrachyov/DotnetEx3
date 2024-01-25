namespace Domain.TicTacToe.Exceptions;

public class ActionRefusedGameException : TicTacToeGameException
{
    public ActionRefusedGameException() : base("Incorrect action.") { }
}
