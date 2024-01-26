using Shared.CQRS;

namespace Features.GameRooms.Queries;

public record GetGameRoomsQuery(int Page = 1, int Limit = 16) : IQuery<IEnumerable<GameRoomDto>>;
