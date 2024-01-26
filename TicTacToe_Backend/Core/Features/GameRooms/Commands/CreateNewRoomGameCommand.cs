using Shared.CQRS;

namespace Features.Games.Commands;

public record CreateNewRoomGameCommand(int MaxUserRating, string CreatorId, string CreatorUserName) : ICommand<Guid>;
