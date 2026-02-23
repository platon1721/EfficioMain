using Base.Contracts;

namespace Base.Domain;

public abstract class BaseEntity : BaseEntity<Guid>, IDomainId
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    protected BaseEntity(Guid id)
    {
        Id = id;
    }
}

public abstract class BaseEntity<TKey> : IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; protected set; } =  default!;
    
}