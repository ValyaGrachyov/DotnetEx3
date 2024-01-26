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
    private readonly IUserRepository _userRepository;

    public JoinRoomCommandHandler(IGameRoomRepository gameRoomRepository, ITicTacToeGameEngine engine, IUserRepository userRepository, IUpdateRecorder gameEventNotifier)
    {
        _gameRoomRepository = gameRoomRepository;
        _engine = engine;
        _userRepository = userRepository;
        _gameEventNotifier = gameEventNotifier;
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
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null || user.Rate >= room.MaxAllowedPlayerRate)
            {
                return Result.ErrorResult;
            }
            
            var events = await _engine.JoinRoomAsync(room, request.UserId, user.UserName );
            foreach (var gameEvent in events)
                await _gameEventNotifier.RecordUpdateAsync(gameEvent);

            return Result.SuccessResult;
        }
        catch (Exception ex)
        {
            return Result.ErrorResult;
        }
        
    }
}
