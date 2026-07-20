using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Security.Module;

public class CreateModuleRequest
{
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = default!;

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public bool IsMain { get; set; }
    public bool IsActive { get; set; } = true;
}