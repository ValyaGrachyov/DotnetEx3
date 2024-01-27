using Domain.TicTacToe;

namespace Features.GameRooms.Queries;

public record GameDto
{
    public TicTacToeSymbols[] GameField { get; init; } = Enumerable.Range(0, 9).Select(x => TicTacToeSymbols.None).ToArray();

    public bool IsPlayer1Turn { get; set; }

    public PlayerDto Player1 { get; set; }

    public PlayerDto Player2 { get; set;}
}

public record PlayerDto
{
    public string Username { get; set; }

    public TicTacToeSymbols Symbol { get; set; }
}