namespace Domain.TicTacToe;

public class TicTacToeGameRoom
{
    public string RoomId { get; init; }

    public int MaxAllowedPlayerRate { get; init; }

    public string RoomCreatorId { get; init; }

    public string? OpponentId { get; set; }

    public TicTacToeGame? CurrnetGame { get; set; }

    public TicTacToeRoomState CurrentGameState { get; set; } = TicTacToeRoomState.WaitingForOpponent;


    public DateTime CreationDateTimeUtc { get; init; }

}