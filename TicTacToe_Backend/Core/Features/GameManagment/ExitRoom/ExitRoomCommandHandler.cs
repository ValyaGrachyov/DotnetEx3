using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.ExitRoom;

public class ExitRoomCommandHandler : ICommandHandler<ExitRoomCommand>
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ITicTacToeGameEngine _engine;
    private readonly IUpdateRecorder _updateRecorder;

    public ExitRoomCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine gameEngine, IUpdateRecorder updateRecorder)
    {
        _gameRoomRepository = gameRoomRepository; 
        _engine = gameEngine;
        _updateRecorder = updateRecorder;
    }

    public async Task<Result> Handle(ExitRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _gameRoomRepository.GetGameRoomByIdAsync(request.RoomId);
        if (room == null)
            return Result.ErrorResult;

        foreach (var @event in await _engine.ExitRoomAsync(room, request.UserId))
            await _updateRecorder.RecordUpdateAsync(@event);

        if (room.CurrentGameState == TicTacToeRoomState.Closed)
            await _gameRoomRepository.RemoveRoomGameByIdAsync(request.RoomId);

        return Result.SuccessResult;
    }
}
