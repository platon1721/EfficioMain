namespace Base.Contracts;


/// <summary>
/// Interface for holding delete information.
/// </summary>
public interface IDomainSoftDelete
{
    public bool IsDeleted { get; }
    
    public Guid? DeletedById { get; }
    public string? DeletedByName {
        get;
    }
    public DateTime? DeletedAt {
        get;
    }
}