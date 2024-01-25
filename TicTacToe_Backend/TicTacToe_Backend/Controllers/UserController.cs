using Microsoft.AspNetCore.Mvc;

namespace TicacToe_Backend.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController: ControllerBase
{
    [HttpGet("test")]
    public async Task<IActionResult> GetUser()
    {
        return Ok("Ok");
    }
    
}