using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Queries;

internal class GetGameRoomsQueryHandler : IQueryHandler<GetGameRoomsQuery, IEnumerable<GameRoomDto>>
{
    private readonly IGameRoomRepository _gameRoomRepository;

    public GetGameRoomsQueryHandler(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task<Result<IEnumerable<GameRoomDto>>> Handle(GetGameRoomsQuery request, CancellationToken cancellationToken)
    {
        var rooms = await _gameRoomRepository.GetGameRooms();
        return new Ok<IEnumerable<GameRoomDto>>(rooms.Select(x => new GameRoomDto()
        {
            Id = x.RoomId.ToString(),
            CreatorUsername = x.CreatorUserName,
            IsBusy = x.CurrentGameState != TicTacToeRoomState.WaitingForOpponent,
            CreatedAtUtc = x.CreationDateTimeUtc,
            MaxAllowedUserRating = x.MaxAllowedPlayerRate
        }));
    }
}
