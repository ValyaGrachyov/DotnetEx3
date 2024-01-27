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
        return _hubContext.Clients.Group(update.RoomId.ToString()).RoomMessage("SERVER", "Starting the game.");
    }

    private Task CommentEventAsync(GameEndEvent update)
    {
        string message = string.Format("{0} won. Restaring after 10s.", update.WinnerName ?? "Nobody");
        return _hubContext.Clients.Group(update.RoomId.ToString()).RoomMessage("SERVER", message);
    }

    //Fall lBack
    private Task CommentEventAsync(TicTacToeGameEvent update)
    {
        return Task.CompletedTask;
    }
}
