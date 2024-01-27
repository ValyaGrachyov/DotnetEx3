using DataAccess;
using Domain.TicTacToe;
using Domain.TicTacToe.Exceptions;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.MakeTurn;

public class MakeTurnCommandHandler : ICommandHandler<MakeTurnCommand>
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ITicTacToeGameEngine _engine;
    private readonly IUpdateRecorder _gameEventNotifier;

    public MakeTurnCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine engine, IUpdateRecorder updater)
    {
        _gameRoomRepository = gameRoomRepository;
        _engine = engine;
        _gameEventNotifier = updater;
    }

    public async Task<Result> Handle(MakeTurnCommand request, CancellationToken cancellationToken)
    {
        var room = await _gameRoomRepository.GetGameRoomByIdAsync(request.RoomId);
        if (room == null)
            return Result.ErrorResult;

        if (room.CurrentGameState != TicTacToeRoomState.Started || room.CurrnetGame is null)
            return Result.ErrorResult;

        var game = room.CurrnetGame!;
        if (game.Player1.UserId != request.UserId && game.Player2.UserId != request.UserId)
            return Result.ErrorResult;

        var isCurrentUserTurn = game.IsPlayer1Turn ? (game.Player1.UserId == request.UserId) : (game.Player2.UserId == request.UserId);
        if (!isCurrentUserTurn)
            return Result.ErrorResult;

        var row = request.Row;
        var column = request.Column;
        if (row is < 0 or > 2 || column is < 0 or > 2 || game.GameField[row * 3 + column] != TicTacToeSymbols.None)
            return Result.ErrorResult;

        var events = await _engine.MakeTurnAsync(game, row, column);
        
        foreach (var gameEvent in events)
            await _gameEventNotifier.RecordUpdateAsync(gameEvent);

        if (game.Winner != null)
            await AfterWinAsync(room);

        return Result.SuccessResult;
    }

    private async Task AfterWinAsync(TicTacToeGameRoom room)
    {
        await _gameRoomRepository.UpdateSessionStatusAsync(room.Id, TicTacToeRoomState.Finished);
        await Task.Delay(TimeSpan.FromSeconds(10));
        room = await _gameRoomRepository.GetGameRoomByIdAsync(room.Id);

        if (room != null && room.CurrentGameState == TicTacToeRoomState.Finished)
        {
            try
            {
                var events = await _engine.RestartGameAsync(room);
                foreach (var gameEvent in events)
                    await _gameEventNotifier.RecordUpdateAsync(gameEvent);
            }
            catch(ActionRefusedGameException)
            {
            }
        }
    }
}
