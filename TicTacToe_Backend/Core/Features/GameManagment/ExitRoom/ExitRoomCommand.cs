using Shared.CQRS;

namespace Features.GameManagment.ExitRoom;

public record ExitRoomCommand(string UserId, string RoomId) : ICommand;
