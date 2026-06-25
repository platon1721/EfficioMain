using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Tenants.TenantModule;

public class AssignTenantModuleRequest
{
    [Required]
    public Guid ModuleId { get; set; }

    public DateTime? ExpiresAt { get; set; }
}