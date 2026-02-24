using Base.Contracts;

namespace Base.Domain;

public abstract class BaseTenantEntity<TKey> : BaseEntity<TKey>, ITenantScoped
    where TKey : IEquatable<TKey>
{
    public Guid TenantRootDepartmentId { get; private set; }
    
    public void SetRootDepartmentId(Guid tenantRootDepartmentId)
        => TenantRootDepartmentId = tenantRootDepartmentId;
}

public abstract class BaseTenantEntity : BaseTenantEntity<Guid>
{
    
}