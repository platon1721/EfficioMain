using Base.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.DTO.Tenants;

public class TenantModule : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime? ExpiresAt { get; set; }

    // Populated when loaded with navigation
    public Module? Module { get; set; }
}