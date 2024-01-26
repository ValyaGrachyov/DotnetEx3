using Shared.CQRS;

namespace Features.Games.Commands;

public record CreateNewRoomGameCommand(int MaxUserRating) : ICommand<string>;
