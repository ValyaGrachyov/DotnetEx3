using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace TicacToe_Backend.Helpers.Authorization;

public interface IJwtGenerator
{
    public string CreateToken(IdentityUser user);
}