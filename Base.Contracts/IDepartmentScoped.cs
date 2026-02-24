namespace Base.Contracts;

/// <summary>
/// Entity that belongs to a specific department.
/// Inherits tenant scope since departments exist within tenants.
/// </summary>
public interface IDepartmentScoped : ITenantScoped
{ 
    Guid DepartmentId { get; }
}
