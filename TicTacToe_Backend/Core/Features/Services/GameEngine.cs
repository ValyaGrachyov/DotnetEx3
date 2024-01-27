using Domain.TicTacToe;
using Domain.TicTacToe.GameEvents;
using DataAccess;
using Domain.TicTacToe.Exceptions;

namespace Features.Services;

public class GameEngine : ITicTacToeGameEngine
{
    private readonly IGameRoomRepository _gamesRepository;
    private readonly IAwarder _awarder;

    public GameEngine(IGameRoomRepository gamesRepository, IAwarder awarder)
    {
        _gamesRepository = gamesRepository;
        _awarder = awarder;
    }

    private async Task InitGameAsync(TicTacToeGameRoom room)
    {
        var updateSessionTask = _gamesRepository.UpdateSessionStatusAsync(room.Id, TicTacToeRoomState.Loading);

        TicTacToePlayer player1;
        TicTacToePlayer player2;
        if (GetRandomBool())
        {
            player1 = new TicTacToePlayer() { UserId = room.RoomCreatorId, Symbol = TicTacToeSymbols.X };
            player2 = new TicTacToePlayer() { UserId = room.OpponentId!, Symbol = TicTacToeSymbols.O };
        }
        else
        {
            player1 = new TicTacToePlayer() { UserId = room.RoomCreatorId, Symbol = TicTacToeSymbols.O };
            player2 = new TicTacToePlayer() { UserId = room.OpponentId!, Symbol = TicTacToeSymbols.X };
        }
        player1.Username = room.CreatorUserName;
        player2.Username = room.OpponentUserName!;

        var game = new TicTacToeGame()
        {
            RoomId = room.Id,
            Player1 = player1,
            Player2 = player2,
            IsPlayer1Turn = player1.Symbol == TicTacToeSymbols.X,
            LastGameActionTimeUtc = DateTime.UtcNow,
        };
        room.CurrnetGame = game;
        room.CurrentGameState = TicTacToeRoomState.Started;
        await updateSessionTask;
        await _gamesRepository.UpdateSessionAsync(room);
    }

    private static readonly Random random = new();
    private static bool GetRandomBool() => random.NextDouble() >= 0.5;

    private static bool NoTurnsLeft(TicTacToeSymbols[] gameField) => !gameField.Any(x => x == TicTacToeSymbols.None);

    private static (bool HasWinningCombination, TicTacToeSymbols WonSymbol) TryFindWinningCombination(TicTacToeSymbols[] gameField)
    {
        var result = TryFindRowWinningCombination(gameField);
        if (result.HasWinningCombination)
            return result;

        result = TryFindColumnWinningCombination(gameField);
        if (result.HasWinningCombination)
            return result;

        return TryFindDiagonalWinningCombination(gameField);
    }

    private static (bool HasWinningCombination, TicTacToeSymbols WonSymbol) TryFindRowWinningCombination(TicTacToeSymbols[] gameField)
    {
        for (int row = 0; row < 3; row++)
        {
            var offset = row * 3;
            if (gameField[offset] == TicTacToeSymbols.None)
                continue;

            if (gameField[offset] == gameField[offset + 1] && gameField[offset + 1] == gameField[offset + 2])
                return (true, gameField[offset]);
        }

        return (false, TicTacToeSymbols.None);
    }

    private static (bool HasWinningCombination, TicTacToeSymbols WonSymbol) TryFindColumnWinningCombination(TicTacToeSymbols[] gameField)
    {
        for (int column = 0; column < 3; column++)
        {
            if (gameField[column] == TicTacToeSymbols.None)
                continue;

            if (gameField[column] == gameField[3 + column] && gameField[3 + column] == gameField[6 + column])
                return (true, gameField[column]);
        }

        return (false, TicTacToeSymbols.None);
    }

    private static (bool HasWinningCombination, TicTacToeSymbols WonSymbol) TryFindDiagonalWinningCombination(TicTacToeSymbols[] gameField)
    {
        if (gameField[0] != TicTacToeSymbols.None && gameField[0] == gameField[4] && gameField[4] == gameField[8])
            return (true, gameField[0]);

        if (gameField[2] != TicTacToeSymbols.None && gameField[2] == gameField[4] && gameField[4] == gameField[6])
            return (true, gameField[2]);

        return (false, TicTacToeSymbols.None);
    }

    public async Task<IEnumerable<TicTacToeGameEvent>> MakeTurnAsync(TicTacToeGame game, int row, int column)
    {
        var events = new List<TicTacToeGameEvent>();

        var player = game.IsPlayer1Turn ? game.Player1 : game.Player2;

        int argIndex = row * 3 + column;
        game.GameField[argIndex] = player.Symbol;

        events.Add(new PutSymbolGameEvent()
        {
            Column = column,
            Row = row,
            PutSymbol = player.Symbol,
            RoomId = game.RoomId,
            UserId = player.UserId,
            Username = player.Username
        });

        (bool HasWinningCombination, TicTacToeSymbols WonSymbol) = TryFindWinningCombination(game.GameField);

        if (HasWinningCombination)
        {
            game.Winner = game.Player1.Symbol == WonSymbol ? Winner.Player1 : Winner.Player2;
            events.Add(new GameEndEvent()
            {
                RoomId = game.RoomId,
                WinnerId = game.Winner == Winner.Player1 ? game.Player1.UserId : game.Player2.UserId,
                WinnerName = game.Winner == Winner.Player1 ? game.Player1.Username : game.Player2.Username,
            });

            await _awarder.ChangeUserRateAsync(game.Winner == Winner.Player1 ? game.Player1.UserId : game.Player2.UserId, 3);
            await _awarder.ChangeUserRateAsync(game.Winner != Winner.Player1 ? game.Player1.UserId : game.Player2.UserId, -1);
        }
        else if (NoTurnsLeft(game.GameField))
        {
            game.Winner = Winner.Nobody;

            events.Add(new GameEndEvent()
            {
                RoomId = game.RoomId,
                WinnerId = default,
                WinnerName = default
            });
        }
        else
        {
            game.IsPlayer1Turn = !game.IsPlayer1Turn;
        }

        game.LastGameActionTimeUtc = DateTime.UtcNow;
        await _gamesRepository.UpdateRoomGameAsync(game.RoomId, game);

        return events;
    }

    public async Task<IEnumerable<TicTacToeGameEvent>> ExitRoomAsync(TicTacToeGameRoom room, string userId)
    {
        var events = new List<TicTacToeGameEvent>();

        if (room.CurrentGameState != TicTacToeRoomState.Finished || room.CurrentGameState != TicTacToeRoomState.WaitingForOpponent)
        {
            events.Add(new GameEndEvent()
            {
                RoomId = room.Id,
                WinnerId = room.OpponentId == userId ? room.RoomCreatorId : room.OpponentId,
                WinnerName = room.OpponentId == userId ? room.CreatorUserName : room.OpponentUserName,
            });
            await _awarder.ChangeUserRateAsync(userId, -3);
        }

        if (room.RoomCreatorId == userId)
        {
            room.CurrentGameState = TicTacToeRoomState.WaitingForOpponent;
            room.OpponentId = null;
            room.OpponentUserName = null;
        }
        else
        {
            room.CurrentGameState = TicTacToeRoomState.Closed;
        }
        room.CurrnetGame = null;

        await _gamesRepository.UpdateSessionAsync(room);

        return Array.Empty<TicTacToeGameEvent>();
    }

    public Task<IEnumerable<TicTacToeGameEvent>> JoinRoomAsync(TicTacToeGameRoom room, string userId, string userName)
    {
        room.OpponentId = userId;
        room.OpponentUserName = userName;
        return StartGameAsync(room);
    }

    private async Task<IEnumerable<TicTacToeGameEvent>> StartGameAsync(TicTacToeGameRoom room)
    {
        await InitGameAsync(room);
        var game = room.CurrnetGame!;
        return new TicTacToeGameEvent[]
        {
            new NewGameStartEvent()
                {
                    RoomId = room.Id,
                    IsPlayer1Turn = game.IsPlayer1Turn,
                    Player1 = game.Player1,
                    Player2 = game.Player2,
                }
        };
    }

    public Task<IEnumerable<TicTacToeGameEvent>> RestartGameAsync(TicTacToeGameRoom room)
    {
        if (room.CurrentGameState != TicTacToeRoomState.Finished || room.OpponentId == null)
            throw new ActionRefusedGameException();

        return StartGameAsync(room);
    }
}
