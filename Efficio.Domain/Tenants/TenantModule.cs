using Base.Domain;

namespace Efficio.Domain.Tenants;

public class TenantModule: BaseSoftDeleteTenantEntity
{
    public Guid TenantId { get; set; }
    public Guid ModuleId { get; set; }
    
    public DateTime ExpiresAt { get; set; }
}