namespace Domain.TicTacToe;

public interface ITicTacToeGameCore
{
    /// <summary>
    /// Creates game proccessor for specific user.
    /// </summary>
    /// <param name="gameSession"></param>
    /// <param name="applicantId"></param>
    /// <returns></returns>
    public ITicTacToeGameProccessor? GetGameProccessor(string gameSession, string applicantId);

    public void KeepALiveSession(string gameSession);
}