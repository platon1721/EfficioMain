using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Efficio.Domain.Security;

public class Permission : BaseSoftDeleteEntity
{
    public Guid ModuleId { get; set; }
    [Required]
    [MaxLength(30)]
    public string Key { get; set; } = default!;
    public string? Name { get; set; }
    public bool IsActive { get; set; } = true;
    
    public Module? Module { get; set; }

}