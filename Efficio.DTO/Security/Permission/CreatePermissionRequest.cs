using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Security.Permission;

public class CreatePermissionRequest
{
    [Required]
    public Guid ModuleId { get; set; }

    [Required]
    [MaxLength(30)]
    public string Key { get; set; } = default!;

    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
}