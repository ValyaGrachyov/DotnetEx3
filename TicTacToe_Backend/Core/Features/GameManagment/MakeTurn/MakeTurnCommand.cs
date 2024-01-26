using Shared.CQRS;

namespace Features.GameManagment.MakeTurn;

public record MakeTurnCommand(Guid RoomId, string UserId, int Row, int Column) : ICommand;
