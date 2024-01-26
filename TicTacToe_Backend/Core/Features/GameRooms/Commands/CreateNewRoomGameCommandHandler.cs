using DataAccess;
using Features.Games.Commands;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Commands;

public class CreateNewRoomGameCommandHandler : ICommandHandler<CreateNewRoomGameCommand, string>
{

    private readonly IGameRoomRepository _gameRoomRepository;
    

    public CreateNewRoomGameCommandHandler(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task<Result<string>> Handle(CreateNewRoomGameCommand request, CancellationToken cancellationToken)
    {
       var roomId = await _gameRoomRepository.CreateRoom(request.MaxUserRating, request.CreatorId, request.CreatorUserName);
       return new Ok<string>(roomId);
    }
}
