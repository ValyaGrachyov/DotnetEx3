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
        if (update is NewGameStartEvent)
            await _hubContext.Clients.Group(update.RoomId.ToString()).RoomMessage("SERVER", "Starting the game.");
        await _hubContext.Clients.Group(update.RoomId.ToString()).GameEvent(update);
    }
}
