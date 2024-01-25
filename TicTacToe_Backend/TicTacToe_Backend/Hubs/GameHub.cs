using Domain.TicTacToe;
using Features.GameManagment.MakeTurn;
using MediatR;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace TicacToe_Backend.Hubs;

[Authorize]
public class GameHub : Microsoft.AspNet.SignalR.Hub
{
    private readonly IMediator _mediator;

    public GameHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task SubscribeRoomEvents(string roomId)
    {
        Groups.Add(Context.ConnectionId, roomId);
        //todo: validate if room exists
        // assign connection to group
    }

    public async Task UnsubscribeRoomEvents(string roomId)
    {
        Groups.Remove(Context.ConnectionId, roomId);
        //todo: validate if room exists
        // assign connection to group
    }

    public async Task MakeTurn(string roomId, int row, int column)
    {
        var userId = "someId";
        var makeTurnResult = await _mediator.Send(new MakeTurnCommand(roomId, userId, row, column));
        if (!makeTurnResult.IsSuccess)
            throw new NotImplementedException();
            //todo: response only to this connection
    }
    //todo: create Event send function
    // event notifier - create service wich implements IUpdater
    // also need 
}
