namespace Base.Contracts;

/// <summary>
/// Entity that belongs to a specific tenant.
/// Used for automatic tenant filtering in repositories.
/// </summary>
public interface ITenantScoped
{ 
    Guid TenantRootDepartmentId { get; }
}