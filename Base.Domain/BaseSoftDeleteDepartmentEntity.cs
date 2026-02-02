using Base.Contracts;

namespace Base.Domain;

public abstract class BaseSoftDeleteDepartmentEntity<TKey> : BaseAuditableDepartmentEntity<TKey>, IDomainSoftDelete
    where TKey : IEquatable<TKey>
{
    public bool IsDeleted { get; private set; }
    public Guid? DeletedById { get; private set; }
    public string? DeletedByName { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Delete(Guid userId)
    {
        IsDeleted = true;
        DeletedById = userId;
        DeletedAt = DateTime.UtcNow;
    }
    
    public void DeleteWithName(string name)
    {
        DeletedByName = name;
    }
}

public abstract class BaseSoftDeleteDepartmentEntity : BaseSoftDeleteDepartmentEntity<Guid>
{
    
}