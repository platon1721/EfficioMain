using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Auth.RefreshToken;

public class RefreshTokenRequest
{
    [Required]
    public string Token { get; set; } = default!;

    [Required]
    public string RefreshToken { get; set; } = default!;
}