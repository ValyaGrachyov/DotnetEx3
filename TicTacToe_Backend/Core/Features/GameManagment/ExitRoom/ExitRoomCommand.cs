using Shared.CQRS;

namespace Features.GameManagment.ExitRoom;

public record ExitRoomCommand(string UserId, Guid RoomId) : ICommand;
