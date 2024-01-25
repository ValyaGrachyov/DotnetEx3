using Features.Games.Commands;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Commands;

public class CreateNewRoomGameCommandHandler : ICommandHandler<CreateNewRoomGameCommand, string>
{
    public Task<Result<string>> Handle(CreateNewRoomGameCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
