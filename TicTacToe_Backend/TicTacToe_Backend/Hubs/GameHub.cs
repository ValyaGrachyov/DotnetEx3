using Domain.TicTacToe.GameEvents;
using Features.GameManagment.MakeTurn;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TicacToe_Backend.Helpers.Extensions;

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
    }

    public async Task UnsubscribeRoomEvents(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        await Clients.Group(roomId).RoomMessage("SERVER", $"{GetCurrentUsername()} left the room.");
    }

    public async Task MakeTurn(string roomId, int row, int column)
    {
        if (!Guid.TryParse(roomId, out var roomGuid))
            return;

        var userId = GetCurrentUserId();
        var makeTurnResult = await _mediator.Send(new MakeTurnCommand(roomGuid, userId, row, column));
        if (!makeTurnResult.IsSuccess)
            await Clients.Client(Context.ConnectionId).GameEvent(new WrongArgumentEvent()
            {
                RoomId = roomGuid,
                ApplicantId = GetCurrentUserId(),
                ApplicantName = GetCurrentUsername(),
                Column = column,
                Row = row,
            });
    }

    public Task SendRoomMessage(string roomId, string message)
    {
        return Clients.Group(roomId).RoomMessage(GetCurrentUsername(), message);
    }

    private string GetCurrentUsername() => Context.User.GetUserUsername()!;

    private string GetCurrentUserId() => Context.User.GetUserId()!;
}
