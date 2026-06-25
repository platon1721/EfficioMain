namespace Efficio.DTO.Auth;

/// <summary>
/// Shared response for Register, Login, and RefreshToken endpoints.
/// </summary>
public class JwtResponse
{
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}