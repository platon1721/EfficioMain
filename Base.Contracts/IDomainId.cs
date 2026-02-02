namespace Base.Contracts;


/// <summary>
/// Generic id for entities. By default is Guid
/// </summary>
public interface IDomainId : IDomainId<Guid>
{
    
}

public interface IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id {
        get;
    }
}