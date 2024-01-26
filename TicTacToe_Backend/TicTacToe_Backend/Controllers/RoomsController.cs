using DataAccess;
using Features.GameManagment.JoinRoomCommand;
using Features.GameRooms.Queries;
using Features.Games.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicacToe_Backend.Controllers;

[ApiController]
[Route("/rooms")]
//[Authorize]
public class RoomsController: ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public RoomsController(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom([FromBody] int maxRate )
    {
        var user = await _userRepository.GetUserByIdAsync(HttpContext.User.Claims.FirstOrDefault().Value);
        var result = await _mediator.Send(new CreateNewRoomGameCommand(maxRate,
                                    HttpContext.User.Claims.FirstOrDefault().Value,
                                            user.UserName));
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoomInfo([FromRoute]string id)
    {
        var room = await _mediator.Send(new GetGameRoomQuery(id));
        return Ok(new JsonResult(room.Value));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsInfo([FromQuery]int page = 0, [FromQuery]int limit = 16)
    {
        return Ok(new List<GameRoomDto>()
        {
            new GameRoomDto()
            {
                Id = "12345",
                CreatedAtUtc = DateTime.Now,
                CreatorUsername = "test",
                MaxAllowedUserRating = 25,
                IsBusy = false
            },
            new GameRoomDto()
            {
                Id = "09876",
                CreatedAtUtc = DateTime.MinValue,
                CreatorUsername = "player",
                MaxAllowedUserRating = 10,
                IsBusy = false
            }
        });
        var room = await _mediator.Send(new GetGameRoomsQuery());
        return Ok(new JsonResult(room));
    }

    [HttpPost("{id}/join")]
    public async Task<IActionResult> Join([FromRoute]string id)
    {
        var result = await _mediator.Send(new JoinRoomCommand(HttpContext.User.Claims.FirstOrDefault().Value,id));
        return Ok(id);
    }

    [HttpPost("{id}/exit")]
    public async Task<IActionResult> Exit([FromRoute]string id)
    {
        return Ok(id);
    }
}