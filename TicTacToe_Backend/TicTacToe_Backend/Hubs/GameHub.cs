using Domain.TicTacToe.GameEvents;
using Domain.UserStatistics;
using Features.GameManagment.MakeTurn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace TicacToe_Backend.Hubs;

[Authorize]
public class GameHub : Hub<IGameEventsReciever>
{
    private readonly IMediator _mediator;

    public GameHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SubscribeRoomEvents(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        await Clients.Group(roomId).RoomMessage("SERVER", $"{GetCurrentUsername()} joined the room.");
        //todo: validate if room exists
        // assign connection to group
    }

    public async Task UnsubscribeRoomEvents(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        //todo: validate if room exists
        // assign connection to group

        await Clients.Group(roomId).RoomMessage("SERVER", $"{GetCurrentUsername()} left the room.");
    }

    public async Task MakeTurn(string roomId, int row, int column)
    {
        var userId = "someId";
        var makeTurnResult = await _mediator.Send(new MakeTurnCommand(roomId, userId, row, column));
        if (!makeTurnResult.IsSuccess)
            throw new NotImplementedException();
            //todo: response only to this connection
    }

    public Task SendGameEventAsync(string roomId, TicTacToeGameEvent gameEvent)
    {
        // event notifier - create service wich implements IUpdater
        return Clients.Group(roomId).GameEvent(gameEvent);
    }

    private string GetCurrentUsername() => "username-mock";
}
