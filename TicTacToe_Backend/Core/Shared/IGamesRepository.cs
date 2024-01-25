using Domain.TicTacToe;

namespace DataAccess;

public interface IGamesRepository
{
    public Task<TicTacToeGameSession?> GetGameSessionByIdAsync(string sessionId);

    public Task UpdateSessionStatusAsync(string sessionId, TicTacToeGameState state);

    public Task UpdateSessionAsync(TicTacToeGameSession session);
}