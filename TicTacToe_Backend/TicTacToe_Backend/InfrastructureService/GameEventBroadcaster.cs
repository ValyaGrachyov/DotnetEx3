using Domain.TicTacToe;
using Domain.TicTacToe.GameEvents;
using Microsoft.AspNetCore.SignalR;
using TicacToe_Backend.Hubs;

namespace TicacToe_Backend.InfrastructureService;

public class GameEventBroadcaster : IUpdateRecorder
{
    private readonly IHubContext<GameHub, IGameEventsReciever> _hubContext;


    public GameEventBroadcaster(IHubContext<GameHub, IGameEventsReciever> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task RecordUpdateAsync(TicTacToeGameEvent update)
    {
        await CommentEventAsync(update as dynamic);
        
        await _hubContext.Clients.Group(update.RoomId.ToString()).GameEvent(update);
    }

    private Task CommentEventAsync(NewGameStartEvent update)
    {
        string symbolsIdentityMessage = $"{update.Player1.Username} has {update.Player1.Symbol} and {update.Player2.Username} has {update.Player2.Symbol}";
        string message = string.Format("Starting the game. {0}. {1} has turn.", symbolsIdentityMessage, update.IsPlayer1Turn ? update.Player1.Username : update.Player2.Username);

        return SendRoomMessageAsync(update.RoomId, message);
    }

    private Task CommentEventAsync(WaitingForOpponentGameEvent update) => SendRoomMessageAsync(update.RoomId, "Suspending the game. Waiting for opponnent.");

    private Task CommentEventAsync(GameEndEvent update)
    {
        string message = string.Format("{0} won. Restaring after 10s.", update.WinnerName ?? "Nobody");
        return SendRoomMessageAsync(update.RoomId, message);
    }

    private Task CommentEventAsync(UserJoinRoomEvent update) => SendRoomMessageAsync(update.RoomId, $"{update.Username} joined the game.");

    private Task CommentEventAsync(UserLeftRoomEvent update) => SendRoomMessageAsync(update.RoomId, $"{update.Username} left the game.");

    //Fall lBack
    private Task CommentEventAsync(TicTacToeGameEvent update)
    {
        return Task.CompletedTask;
    }

    private Task SendRoomMessageAsync(Guid roomId, string message) => _hubContext.Clients.Group(roomId.ToString()).RoomMessage("SERVER", message);
}
