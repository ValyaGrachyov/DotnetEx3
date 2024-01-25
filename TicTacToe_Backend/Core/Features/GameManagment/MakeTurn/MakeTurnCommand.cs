using Domain.TicTacToe;
using Shared.CQRS;

namespace Features.GameManagment.MakeTurn;

public record MakeTurnCommand(string RoomId, string UserId, int Row, int Column) : ICommand;
