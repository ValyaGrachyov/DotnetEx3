namespace Domain.TicTacToe;

public interface IAwarder
{
    public Task ChangeUserRateAsync(string userId, int rateDelta);

}
