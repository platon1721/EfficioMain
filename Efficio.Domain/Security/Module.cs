using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Efficio.Domain.Security;

public class Module: BaseEntity
{
    [Required]
    [MaxLength(30)]
    public string Code { get; set; } = default!;
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    public bool IsMain {get; set;} = false;
    public bool IsActive { get; set; }
}