namespace Base.Contracts;

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