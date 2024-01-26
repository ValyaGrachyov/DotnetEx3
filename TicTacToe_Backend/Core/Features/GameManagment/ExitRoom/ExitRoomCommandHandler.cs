using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.ExitRoom;

public class ExitRoomCommandHandler : ICommandHandler<ExitRoomCommand>
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ITicTacToeGameEngine _engine;

    public ExitRoomCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine gameEngine)
    {
        _gameRoomRepository = gameRoomRepository; 
        _engine = gameEngine;
    }

    public async Task<Result> Handle(ExitRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _gameRoomRepository.GetGameRoomByIdAsync(request.RoomId);
        if (room == null)
            return Result.ErrorResult;

        await _engine.ExitRoomAsync(room, request.UserId);

        if (room.CurrentGameState == TicTacToeRoomState.Closed)
            await _gameRoomRepository.RemoveRoomGameByIdAsync(request.RoomId);

        return Result.SuccessResult;
    }
}
