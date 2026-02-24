using Base.Contracts;

namespace Base.Domain;

public abstract class BaseDepartmentScopedEntity<TKey> : BaseTenantEntity<TKey>, IDepartmentScoped
    where TKey : IEquatable<TKey>
{
    public Guid DepartmentId { get; private set; }
    
    public void SetDepartmentId(Guid departmentId) => DepartmentId = departmentId;
}

public abstract class BaseDepartmentScopedEntity : BaseDepartmentScopedEntity<Guid>
{
    
}