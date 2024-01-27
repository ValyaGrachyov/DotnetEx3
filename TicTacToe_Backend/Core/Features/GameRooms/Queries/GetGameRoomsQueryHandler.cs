using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Queries;

internal class GetGameRoomsQueryHandler : IQueryHandler<GetGameRoomsQuery, PaginatedGameRoomsInfoDto>
{
    private readonly IGameRoomRepository _gameRoomRepository;

    public GetGameRoomsQueryHandler(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task<Result<PaginatedGameRoomsInfoDto>> Handle(GetGameRoomsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var roomsTask = _gameRoomRepository.GetActiveGameRoomsAsync(request.Page, request.Limit);
            var totalCountTask = _gameRoomRepository.GetGameRoomsCountAsync();
            await Task.WhenAll(roomsTask, totalCountTask);
            
            var roomInfos = roomsTask!.Result!.Select(x =>
            {
                var game = x.CurrnetGame;
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

                return new GameRoomDto()
                {
                    Id = x.Id.ToString(),
                    CreatorUsername = x.CreatorUserName,
                    OpponentUsername = x.OpponentUserName,
                    IsBusy = x.CurrentGameState != TicTacToeRoomState.WaitingForOpponent,
                    CreatedAtUtc = x.CreationDateTimeUtc,
                    MaxAllowedUserRating = x.MaxAllowedPlayerRate,
                    CurrentGame = dto
                };
            });

            return new Ok<PaginatedGameRoomsInfoDto>(new PaginatedGameRoomsInfoDto()
            {
                RequestedRooms = roomInfos,
                TotalRooms = totalCountTask.Result
            });

        }
        catch
        {
            return new Error<PaginatedGameRoomsInfoDto>();
        }
    }
}
