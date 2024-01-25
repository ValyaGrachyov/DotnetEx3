using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Queries;

internal class GetGameRoomsQueryHandler : IQueryHandler<GetGameRoomsQuery, IEnumerable<GameRoomDto>>
{
    public Task<Result<IEnumerable<GameRoomDto>>> Handle(GetGameRoomsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
