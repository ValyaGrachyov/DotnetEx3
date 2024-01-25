using Shared;
using Domain;
using Domain.TicTacToe;
using Domain.TicTacToe.Exceptions;
using Domain.TicTacToe.GameEvents;
using DataAccess;

namespace Features.Games.Service
{
    public class GameCore : ITicTacToeGameCore
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUpdateRecorder _updateRecorder;

        private static readonly Random random = new Random();
        private static bool GetRandomBool() => random.NextDouble() >= 0.5;

        private static bool IsValidPutAction(TicTacToeSymbols[] gameField, int argIndex)
        {
           return argIndex is >= 0 and < 9 && gameField[argIndex] == TicTacToeSymbols.None;
        }

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


        public GameCore(IGamesRepository gamesRepository, IUpdateRecorder updateRecorder)
        {
            _gamesRepository = gamesRepository;
            _updateRecorder = updateRecorder;
        }

        public async Task<ITicTacToeGameProccessor> GetGameProccessor(string gameSession, string applicantId)
        {
            var session = await _gamesRepository.GetGameSessionByIdAsync(gameSession);
            if (session == null)
                throw new GameNotFoundException();

            var user = await _userRepository.GetUserByIdAsync(applicantId);
            if (user == null)
                throw new UserNotFoundException();

            return new GameProccessor(applicantId, _updateRecorder, _gamesRepository);
        }

        public void KeepALiveSession(string gameSession)
        {
            throw new NotImplementedException();
        }

        ITicTacToeGameProccessor? ITicTacToeGameCore.GetGameProccessor(string gameSession, string applicantId)
        {
            throw new NotImplementedException();
        }

        private class GameProccessor : ITicTacToeGameProccessor
        {
            private readonly string _applicantId;

            private TicTacToeGameSession _session;

            private readonly IUpdateRecorder _updateRecorder;

            private readonly IGamesRepository _gamesRepository;

            public GameProccessor(string userId, IUpdateRecorder updateRecorder, IGamesRepository gamesRepository)
            {
                _applicantId = userId;
                _updateRecorder = updateRecorder;
                _gamesRepository = gamesRepository;
            }

            public Task ExitGameAsync()
            {
                // todo: check if game is finished if it is not then punish exiter as loose.
                // close the game if creator leaves the room or send opponent left event
                throw new NotImplementedException();
            }

            public async Task JoinGameAsync()
            {
                if (_session.GameCreatorId == _applicantId)
                    // case when user tries to join created room
                    throw new ActionRefusedGameException();

                _session = await _gamesRepository.GetGameSessionByIdAsync(_session.GameSessionId) ?? throw new AccessDeniedGameException();

                //todo: check if applicant rating is fit
                if (_session.CurrentGameState == TicTacToeGameState.WaitingForOpponent)
                {
                    await InitGameAsync(_session);

                    var currentGame = _session.CurrnetGame!;
                    await _updateRecorder.RecordUpdateAsync(new NewGameStartEvent()
                    {
                        GameSessionId = _session.GameSessionId,
                        IsPlayer1Turn = currentGame.IsPlayer1Turn,
                        Player1 = currentGame.Player1,
                        Player2 = currentGame.Player2,
                    });
                    return;
                }

                throw new ActionRefusedGameException();
            }

            public async Task MakeTurnAsync(int row, int column)
            {
                if (_session.CurrentGameState != TicTacToeGameState.Started)
                    throw new ActionRefusedGameException();

                var currentGame = _session.CurrnetGame!;
                var player = currentGame.IsPlayer1Turn ? currentGame.Player1 : currentGame.Player2;

                if (player.UserId != _applicantId)
                    throw new AccessDeniedGameException();

                int argIndex = row * 3 + column;
                TicTacToeGameEvent gameEvent;
                if (!IsValidPutAction(currentGame.GameField, argIndex))
                    gameEvent = new WrongArgumentEvent()
                    {
                        SessionId = _session.GameSessionId,
                        ApplicantId = _applicantId,
                        Column = column,
                        GameState = _session.CurrentGameState,
                        IsApplicantTurn = currentGame.IsPlayer1Turn ? (currentGame.Player1.UserId == _applicantId) : (currentGame.Player2.UserId == _applicantId),
                        Row = row
                    };
                else
                {
                    currentGame.GameField[argIndex] = currentGame.IsPlayer1Turn ? currentGame.Player1.Symbol : currentGame.Player2.Symbol;
                    currentGame.IsPlayer1Turn = !currentGame.IsPlayer1Turn;
                    currentGame.LastGameActionTimeUtc = DateTime.UtcNow;
                    await _gamesRepository.UpdateSessionAsync(_session);
                    
                    gameEvent = new PutSymbolGameEvent()
                    {
                        SessionId = _session.GameSessionId,
                        Column = column,
                        PutSymbol = currentGame.GameField[argIndex],
                        Row = row
                    };
                }
                var recordUpdateTask = _updateRecorder.RecordUpdateAsync(gameEvent);

                var winCombinationFindResult = TryFindWinningCombination(currentGame.GameField);

                await recordUpdateTask;
                if (winCombinationFindResult.HasWinningCombination)
                {
                    gameEvent = new GameEndEvent()
                    {
                        SessionId = _session.GameSessionId,
                        WinnerId = currentGame.Player1.Symbol == winCombinationFindResult.WonSymbol ? currentGame.Player1.UserId : currentGame.Player2.UserId
                    };
                    //todo: update rating
                }
                else if (NoTurnsLeft(currentGame.GameField))
                {
                    gameEvent = new GameEndEvent()
                    {
                        SessionId = _session.GameSessionId,
                        WinnerId = default
                    };
                    _session.CurrentGameState = TicTacToeGameState.RestartCooldown;
                    _session.CurrnetGame = default;
                    await _gamesRepository.UpdateSessionAsync(_session);
                }
                else
                    gameEvent = new TurnSwitchEvent()
                    {
                        SessionId = _session.GameSessionId,
                        WaitingForUserId = currentGame.IsPlayer1Turn ? currentGame.Player1.UserId : currentGame.Player2.UserId
                    };

                await _updateRecorder.RecordUpdateAsync(gameEvent);
            }

            private async Task InitGameAsync(TicTacToeGameSession session)
            {
                var updateSessionTask = _gamesRepository.UpdateSessionStatusAsync(session.GameSessionId, TicTacToeGameState.Loading);

                TicTacToePlayer player1;
                TicTacToePlayer player2;
                if (GetRandomBool())
                { 
                    player1 = new TicTacToePlayer() { UserId = _session.GameCreatorId, Symbol = TicTacToeSymbols.X };
                    player2 = new TicTacToePlayer() { UserId = _applicantId, Symbol = TicTacToeSymbols.O };
                }
                else
                {
                    player1 = new TicTacToePlayer() { UserId = _session.GameCreatorId, Symbol = TicTacToeSymbols.O };
                    player2 = new TicTacToePlayer() { UserId = _applicantId, Symbol = TicTacToeSymbols.X };
                }


                var game = new TicTacToeGameInfo()
                {
                    Player1 = player1,
                    Player2 = player2,
                    IsPlayer1Turn = player1.Symbol == TicTacToeSymbols.X,
                    LastGameActionTimeUtc = DateTime.UtcNow,
                };

                _session.CurrnetGame = game;
                _session.OpponentId = _applicantId;
                _session.CurrentGameState = TicTacToeGameState.Started;

                await updateSessionTask;

                await _gamesRepository.UpdateSessionAsync(_session);
            }
        }
    }
}
