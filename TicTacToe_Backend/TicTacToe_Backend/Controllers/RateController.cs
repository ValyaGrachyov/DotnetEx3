using System.Formats.Asn1;
using DataAccess;
using Features.UserRate.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicacToe_Backend.Controllers;

[ApiController]
[Authorize]
[Route("/rate")]
public class RateController: ControllerBase
{
    private readonly IMediator _mediator;

    public RateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetUsersRate()
    {
        var usersRate = await _mediator.Send(new GetUsersRateQuery());
        return new JsonResult(usersRate);
    }    
    
}