﻿using DataAccess;
using Features.GameManagment.JoinRoomCommand;
using Features.GameRooms.Queries;
using Features.Games.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicacToe_Backend.Controllers;

[ApiController]
[Route("/rooms")]
[Authorize]
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
    public async Task<IActionResult> CreateRoom([FromBody] int maxRate)
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
        return new JsonResult(room.Value);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRoomsInfo([FromQuery]int page = 0, [FromQuery]int limit = 16)
    {
        var room = await _mediator.Send(new GetGameRoomsQuery());
        return new JsonResult(room);
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