using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.JoinRoomCommand;

public class JoinRoomCommandHandler : ICommandHandler<JoinRoomCommand>
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ITicTacToeGameEngine _engine;
    private readonly IUpdateRecorder _gameEventNotifier;

    public JoinRoomCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine engine)
    {
        _gameRoomRepository = gameRoomRepository;
        _engine = engine;
    }

    public async Task<Result> Handle(JoinRoomCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.RoomId, out var roomId))
            return Result.ErrorResult;

        var room = await _gameRoomRepository.GetGameRoomByIdAsync(roomId);
        if (room == null)
            return Result.ErrorResult;

        if (room.RoomCreatorId == request.UserId)
            return Result.ErrorResult;

        if (room.CurrentGameState != TicTacToeRoomState.WaitingForOpponent)
            return Result.ErrorResult;

        //todo: check for user max rating and reject if not satisfy
        try
        {

        }
        catch (Exception ex)
        {

        }
        var events = await _engine.JoinRoomAsync(room, request.UserId);
        foreach (var gameEvent in events)
            await _gameEventNotifier.RecordUpdateAsync(gameEvent);

        return Result.SuccessResult;
    }
}
