using Base.Contracts;

namespace Base.Domain;

public abstract class BaseAuditableEntity : BaseAuditableEntity<Guid>
{
    
}

public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IDomainMeta 
    where TKey : IEquatable<TKey>
{

    public Guid CreatedById { get; private set; } = default!;
    public string? CreatedByName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid? UpdatedById { get; private set; }
    public string? UpdatedByName { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    public string? SysNotes { get; protected set; }
    
    public void Create(Guid userId, IClock clock)
    {
        CreatedById = userId;
        CreatedAt = clock.UtcNow;
    }

    public void CreateWithName(string name)
    {
        CreatedByName = name;
    }
    
    public void Update(Guid userId, IClock clock)
    {
        UpdatedById = userId;
        UpdatedAt = clock.UtcNow;
    }

    public void UpdateWithName(string name)
    {
        UpdatedByName = name;
    }
}

