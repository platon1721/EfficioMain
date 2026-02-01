namespace Base.Contracts;

public interface IDomainSoftDelete
{
    public bool IsDeleted { get; }
    public string? DeletedBy {
        get;
    }
    public DateTime? DeletedAt {
        get;
    }
}