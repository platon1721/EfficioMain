using Base.Contracts;

namespace Base.Domain;

public abstract class BaseSoftDeleteTenantEntity<TKey> : BaseAuditableTenantEntity<TKey>, IDomainSoftDelete
    where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; private set; }
    public Guid? DeletedById { get; private set; }
    public string? DeletedByName { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Delete(Guid userId, IClock clock)
    {
        IsDeleted = true;
        DeletedById = userId;
        DeletedAt = clock.UtcNow;
    }
    
    public void DeleteWithName(string name)
    {
        DeletedByName = name;
    }
}

public abstract class BaseSoftDeleteTenantEntity : BaseSoftDeleteTenantEntity<Guid>
{
    
}