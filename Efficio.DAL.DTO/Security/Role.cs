using Base.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.DTO.Security;

public class Role : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    
    // Navigation
    public Department? Department { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; }
}