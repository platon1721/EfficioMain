using Base.Domain;
using Efficio.Domain.Security;

namespace Efficio.Domain.Tenants;

public class TenantModule: BaseSoftDeleteTenantEntity
{
    public Guid ModuleId { get; set; }
    
    
    public Module? Module { get; set; }
    public DateTime? ExpiresAt { get; set; }
}