using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicacToe_Backend.Helpers.Authorization;

namespace TicacToe_Backend.Controllers;

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
    public async Task<IActionResult> GenerateToken(UserRegisterVm vm)
    {
        var result = await _userManager.CreateAsync(new IdentityUser()
        {
           UserName = vm.UserName,
        }, vm.Password!);

        if (result.Succeeded)
        {
           return Ok("Created");
        }

        return BadRequest();
    }
    
    
    [HttpPost("login")]
    public async Task<IActionResult> GetUser(UserRegisterVm vm)
    {
        
        var user = await _userManager.FindByNameAsync(vm.UserName!);

        if (user == null)
        {
            return NotFound();
        }
        
        var checkPass = await _signInManager.CheckPasswordSignInAsync(user, vm.Password!, false);
            
        if (!checkPass.Succeeded)
        {
            return NotFound();
        }

        var token = _jwtGenerator.CreateToken(user);
        
        return Ok(token);
    }

    [Authorize]
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("Success");
    }
    
    

    
}