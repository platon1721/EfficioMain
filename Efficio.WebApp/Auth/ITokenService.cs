using System.Security.Claims;
using Efficio.Domain.Identity;

namespace Efficio.WebApp.Auth;

public interface ITokenService
{
    (string jwt, DateTime expiresAt) GenerateJwt(AppUser user);
    AppRefreshToken CreateRefreshToken(Guid userId);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    Task RevokeOldRefreshTokensAsync(Guid userId, int keepCount = 5);
}