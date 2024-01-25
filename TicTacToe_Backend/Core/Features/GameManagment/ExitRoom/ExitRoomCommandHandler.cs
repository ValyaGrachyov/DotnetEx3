using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.ExitRoom;

internal class ExitRoomCommandHandler : ICommandHandler<ExitRoomCommand>
{
    public Task<Result> Handle(ExitRoomCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
