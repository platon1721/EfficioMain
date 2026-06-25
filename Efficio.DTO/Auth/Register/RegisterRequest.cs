using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Auth.Register;

public class RegisterRequest
{
    [Required]
    [MaxLength(128)]
    public string Email { get; set; } = default!;

    [Required]
    [MinLength(6)]
    [MaxLength(128)]
    public string Password { get; set; } = default!;

    [MaxLength(128)]
    public string? FirstName { get; set; }

    [MaxLength(128)]
    public string? LastName { get; set; }
}