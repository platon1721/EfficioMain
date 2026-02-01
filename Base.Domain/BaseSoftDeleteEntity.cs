using Base.Contracts;

namespace Base.Domain;

public abstract class BaseSoftDeleteEntity : BaseSoftDeleteEntity<Guid>
{
    
}

public abstract class BaseSoftDeleteEntity<TKey> : BaseAuditableEntity<TKey>, IDomainSoftDelete
    where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; private set; }
    public string? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Delete(string user)
    {
        IsDeleted = true;
        DeletedBy = user;
        DeletedAt = DateTime.UtcNow;
    }

}