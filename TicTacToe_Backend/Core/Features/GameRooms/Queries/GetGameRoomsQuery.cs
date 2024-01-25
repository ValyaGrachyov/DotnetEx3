using Shared.CQRS;

namespace Features.GameRooms.Queries;

public record GetGameRoomsQuery(int Page = 0, int Limit = 16) : IQuery<IEnumerable<GameRoomDto>>;
