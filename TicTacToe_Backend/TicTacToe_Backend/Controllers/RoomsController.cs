using DataAccess;
using Features.GameManagment.ExitRoom;
using Features.GameManagment.JoinRoomCommand;
using Features.GameRooms.Queries;
using Features.Games.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicacToe_Backend.Helpers.Extensions;

namespace TicacToe_Backend.Controllers;

[ApiController]
[Route("/games")]
[Authorize]
public class RoomsController: ControllerBase
{
    private readonly IMediator _mediator;

    public RoomsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] int maxRate)
    {
        if (maxRate <= 0)
        {
            return BadRequest("Invalid rate");
        }

        var result = await _mediator.Send(new CreateNewRoomGameCommand(maxRate,
                                    HttpContext.User.GetUserId()!,
                                            HttpContext.User.GetUserUsername()!));
        if (result.IsSuccess)
            return Ok(result.Value);
        
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoomInfo([FromRoute]Guid id)
    {
        var room = await _mediator.Send(new GetGameRoomQuery(id));
        return new JsonResult(room.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsInfo([FromQuery]int page = 1, [FromQuery]int limit = 16)
    {
        if (page < 0 || limit == 0)
        {
            return new JsonResult(Array.Empty<object>());
        }

        var room = await _mediator.Send(new GetGameRoomsQuery(page, limit));

        if(room.IsSuccess)
            return new JsonResult(room.Value);

        return new JsonResult(room.ErrorMessage);
    }

    [HttpPost("{id:guid}/join")]
    public async Task<IActionResult> Join([FromRoute]Guid id)
    {
        var result = await _mediator.Send(new JoinRoomCommand(HttpContext.User.GetUserId()!, id));
        if (result.IsSuccess)
            return Ok();
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("{id:guid}/exit")]
    public async Task<IActionResult> Exit([FromRoute]Guid id)
    {
        var exitResult = await _mediator.Send(new ExitRoomCommand(HttpContext.User.GetUserId()!, id));
        if(exitResult.IsSuccess)
            return Ok(id);

        return BadRequest(exitResult.ErrorMessage);
    }
}