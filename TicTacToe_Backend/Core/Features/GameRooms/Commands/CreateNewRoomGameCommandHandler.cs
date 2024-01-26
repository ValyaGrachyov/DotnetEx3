using DataAccess;
using Features.Games.Commands;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameRooms.Commands;

public class CreateNewRoomGameCommandHandler : ICommandHandler<CreateNewRoomGameCommand, Guid>
{

    private readonly IGameRoomRepository _gameRoomRepository;
    

    public CreateNewRoomGameCommandHandler(IGameRoomRepository gameRoomRepository)
    {
        _gameRoomRepository = gameRoomRepository;
    }

    public async Task<Result<Guid>> Handle(CreateNewRoomGameCommand request, CancellationToken cancellationToken)
    {
       var roomId = await _gameRoomRepository.AddRoomAsync(request.MaxUserRating, request.CreatorId, request.CreatorUserName);
       return new Ok<Guid>(roomId);
    }
}
