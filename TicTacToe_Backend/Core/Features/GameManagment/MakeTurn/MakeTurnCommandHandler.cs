using DataAccess;
using Domain.TicTacToe;
using Shared.CQRS;
using Shared.Results;

namespace Features.GameManagment.MakeTurn;

public class MakeTurnCommandHandler : ICommandHandler<MakeTurnCommand>
{
    private readonly IGameRoomRepository _gameRoomRepository;
    private readonly ITicTacToeGameEngine _engine;
    private readonly IUpdateRecorder _gameEventNotifier;

    public MakeTurnCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine engine)
    {
        _gameRoomRepository = gameRoomRepository;
        _engine = engine;
    }

    public async Task<Result> Handle(MakeTurnCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.RoomId, out var roomId))
            return Result.ErrorResult;

        var room = await _gameRoomRepository.GetGameRoomByIdAsync(roomId);
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
        var column = request.Row;
        if (row is < 0 or > 2 || column is < 0 or > 2 || game.GameField[row * 3 + column] != TicTacToeSymbols.None)
            return Result.ErrorResult;

        var events = await _engine.MakeTurnAsync(game, row, column);

        foreach (var gameEvent in events)
            await _gameEventNotifier.RecordUpdateAsync(gameEvent);

        if (game.Winner != null)
            OnWin();

        return Result.SuccessResult;
    }

    private void OnWin()
    {
        //restart game
        //increment users rate
        //todo: revalidate user rate after winning
    }
}
