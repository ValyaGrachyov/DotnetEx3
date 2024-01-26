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

    public Task RecordUpdateAsync(TicTacToeGameEvent update)
    {
        //preproccess events and then drop them into hub

        return _hubContext.Clients.Group(update.RoomId.ToString()).GameEvent(update);
    }
}
