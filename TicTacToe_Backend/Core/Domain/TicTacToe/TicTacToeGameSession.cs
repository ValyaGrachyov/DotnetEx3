﻿namespace Domain.TicTacToe;

public class TicTacToeGameSession
{
    public string GameSessionId { get; init; }

    public int MaxAllowedPlayerRate { get; init; }

    public string GameCreatorId { get; init; }

    public string? OpponentId { get; init; }


    public TicTacToeGameState CurrentGameState { get; set; } = TicTacToeGameState.WaitingForOpponent;


    public DateTime CreationDateTimeUtc { get; init; }

}