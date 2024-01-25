using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Queries;

public class GetGameRoomQueryHandler : IQueryHandler<GetGameRoomQuery, GameRoomDto>
{
    public Task<Result<GameRoomDto>> Handle(GetGameRoomQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
