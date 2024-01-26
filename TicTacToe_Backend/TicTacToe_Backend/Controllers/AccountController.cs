using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TicacToe_Backend.Helpers.Authorization;

namespace TicacToe_Backend.Controllers;

public class UserCredits
{
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}

[Route("account")]
[ApiController]
public class AccountController: ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IJwtGenerator _jwtGenerator;

    public AccountController(UserManager<IdentityUser> userManager, IJwtGenerator jwtGenerator, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _jwtGenerator = jwtGenerator;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUserAsync([FromBody] UserCredits credits)
    {
        var result = await _userManager.CreateAsync(new IdentityUser()
        {
           UserName= credits.UserName,
        }, credits.Password);

        if (result.Succeeded)
        {
           return Ok();
        }

        return BadRequest(result.Errors.FirstOrDefault()?.Description);
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync([FromBody] UserCredits credits)
    {
        var user = await _userManager.FindByNameAsync(credits.UserName);

        if (user == null)
        {
            return NotFound();
        }
        
        var checkPass = await _signInManager.CheckPasswordSignInAsync(user, credits.Password, false);
            
        if (!checkPass.Succeeded)
        {
            return Unauthorized();
        }

        var token = _jwtGenerator.CreateToken(user);
        
        return new JsonResult(new { username=user.UserName, token});
    }

    [Authorize]
    [HttpGet("token/check")]
    public IActionResult TestToken() => Ok();
}