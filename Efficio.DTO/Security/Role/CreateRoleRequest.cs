using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Security.Role;

public class CreateRoleRequest
{
    [Required]
    public Guid DepartmentId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }
}