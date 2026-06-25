using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Auth.Login;

public class LoginRequest
{
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}