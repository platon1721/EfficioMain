using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Auth.Logout;

public class LogoutRequest
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}