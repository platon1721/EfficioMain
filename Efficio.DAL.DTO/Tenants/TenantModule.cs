using Base.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.DTO.Tenants;

public class TenantModule : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ModuleId { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    public Module? Module { get; set; }
}