namespace Base.Contracts;

/// <summary>
/// Interface for holding metadata about creation and updates of data
/// </summary>
public interface IDomainMeta
{
    public Guid CreatedById { get; }
    public string? CreatedByName { get;}
    public DateTime CreatedAt { get;}
    
    
    public Guid? UpdatedById { get; }
    public string? UpdatedByName { get;}
    public DateTime? UpdatedAt { get;}
    
    public string? SysNotes { get;}
}