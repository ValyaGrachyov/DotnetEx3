using Shared.CQRS;

namespace Features.GameManagment.JoinRoomCommand;

public record JoinRoomCommand(string UserId, Guid RoomId) : ICommand;
