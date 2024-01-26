using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Queries;

public class GetGameRoomQueryHandler : IQueryHandler<GetGameRoomQuery, GameRoomDto>
{
    private readonly IGameRoomRepository _gameRoomRepository;

    public GetGameRoomQueryHandler(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task<Result<GameRoomDto>> Handle(GetGameRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _gameRoomRepository.GetGameRoomByIdAsync(request.RoomId);

        if (room == null) return new Error<GameRoomDto>();


        return new Ok<GameRoomDto>(new GameRoomDto()
        {
            Id = room.Id.ToString(),
            CreatorUsername = room.CreatorUserName,
            IsBusy = room.CurrentGameState != TicTacToeRoomState.WaitingForOpponent,
            CreatedAtUtc = room.CreationDateTimeUtc,
            MaxAllowedUserRating = room.MaxAllowedPlayerRate
        });
    }
}
