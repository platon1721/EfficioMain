using Base.Contracts;

namespace Base.Domain;

public abstract class BaseAuditableDepartmentEntity<TKey> : BaseDepartmentScopedEntity<TKey>, IDomainMeta
    where TKey : IEquatable<TKey>
{
    public Guid CreatedById { get; private set; } = default!;
    public string? CreatedByName { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public Guid? UpdatedById { get; private set; }
    public string? UpdatedByName { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    
    public string? SysNotes { get; protected set; }
    
    public void Create(Guid userId)
    {
        CreatedById = userId;
        CreatedAt = DateTime.UtcNow;
    }

    public void CreateWithName(string name)
    {
        CreatedByName = name;
    }
    
    public void Update(Guid userId)
    {
        UpdatedById = userId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateWithName(string name)
    {
        UpdatedByName = name;
    }
}