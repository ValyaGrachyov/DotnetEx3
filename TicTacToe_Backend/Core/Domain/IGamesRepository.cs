﻿using Domain.TicTacToe;

namespace DataAccess;

public interface IGameRoomRepository
{
    public Task<TicTacToeGameRoom?> GetGameRoomByIdAsync(Guid sessionId);

    public Task<IEnumerable<TicTacToeGameRoom>?> GetActiveGameRoomsAsync(int page, int limit);

    public Task UpdateSessionStatusAsync(Guid sessionId, TicTacToeRoomState state);

    public Task UpdateRoomGameAsync(Guid roomId, TicTacToeGame game);

    public Task UpdateSessionAsync(TicTacToeGameRoom session);

    public Task<Guid> AddRoomAsync(int maxRate, string creatorId, string creatorUserName);

    public Task RemoveRoomGameByIdAsync(Guid id);

    public Task<long> GetGameRoomsCountAsync();
}