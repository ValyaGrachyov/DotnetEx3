namespace Domain.TicTacToe;

public class TicTacToeGameRoom
{
    public Guid RoomId { get; init; }

    public int MaxAllowedPlayerRate { get; init; }

    public string RoomCreatorId { get; init; }
    
    public string CreatorUserName { get; set; }

    public string? OpponentId { get; set; }
    
    public string? OpponentUserName {get; set;}

    public TicTacToeGame? CurrnetGame { get; set; }

    public TicTacToeRoomState CurrentGameState { get; set; } = TicTacToeRoomState.WaitingForOpponent;


    public DateTime CreationDateTimeUtc { get; init; }

}