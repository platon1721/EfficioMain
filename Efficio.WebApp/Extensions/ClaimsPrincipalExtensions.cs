using System.Security.Claims;

namespace Efficio.WebApp.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimTypes.NameIdentifier)
                    ?? throw new UnauthorizedAccessException("User ID claim not found");
        return Guid.Parse(claim.Value);
    }

    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value
               ?? throw new UnauthorizedAccessException("Email claim not found");
    }

    public static bool IsPlatformAdmin(this ClaimsPrincipal user)
    {
        return user.FindFirst("IsPlatformAdmin")?.Value == "true";
    }
}