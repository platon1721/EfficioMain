using Base.Contracts;

namespace Base.Domain;

public abstract class BaseSoftDeleteEntity : BaseSoftDeleteEntity<Guid>
{
    
}

public abstract class BaseSoftDeleteEntity<TKey> : BaseAuditableEntity<TKey>, IDomainSoftDelete
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