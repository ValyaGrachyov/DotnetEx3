using Shared.CQRS;

namespace Features.GameRooms.Queries;

public record GetGameRoomQuery(string RoomId) : IQuery<GameRoomDto>;
