using Shared.CQRS;

namespace Features.GameRooms.Queries;

public record GetGameRoomQuery(Guid RoomId) : IQuery<GameRoomDto>;
