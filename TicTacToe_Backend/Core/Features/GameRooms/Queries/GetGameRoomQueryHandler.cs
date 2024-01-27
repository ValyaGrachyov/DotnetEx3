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

        var game = room.CurrnetGame;
        GameDto? dto;
        if (game != null)
        {
            dto = new GameDto()
            {
                GameField = game.GameField,
                IsPlayer1Turn = game.IsPlayer1Turn,
                Player1 = new PlayerDto()
                {
                    Symbol = game.Player1.Symbol,
                    Username = game.Player1.Username,
                },
                Player2 = new PlayerDto()
                {
                    Symbol = game.Player2.Symbol,
                    Username = game.Player2.Username,
                },
            };
        }
        else
        {
            dto = default(GameDto?);
        }

        return new Ok<GameRoomDto>(new GameRoomDto()
        {
            Id = room.Id.ToString(),
            CreatorUsername = room.CreatorUserName,
            OpponentUsername = room.OpponentUserName,
            IsBusy = room.CurrentGameState != TicTacToeRoomState.WaitingForOpponent,
            CreatedAtUtc = room.CreationDateTimeUtc,
            MaxAllowedUserRating = room.MaxAllowedPlayerRate,
            CurrentGame = dto,
        });
    }
}
