using System.Security.Claims;

namespace TicacToe_Backend.Helpers.Extensions;

public static class ClaimsExtensions
{
    public static string? GetUserId(this ClaimsPrincipal claims) => claims.Claims.FirstOrDefault()?.Value;

    public static string? GetUserUsername(this ClaimsPrincipal claims) => claims.Claims.Skip(1).FirstOrDefault()?.Value;

}
