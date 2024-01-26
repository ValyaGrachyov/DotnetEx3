using DataAccess;
using Features.UserRate.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicacToe_Backend.Controllers;

[ApiController]
[Authorize]
[Route("/rate")]
public class RateController: ControllerBase
{
    //TODO finish rate methods
    // [HttpGet]
    // public async Task<IEnumerable<UsersRateDto>> GetUsersRate()
    // {
    //     return Ok();
    // }
}