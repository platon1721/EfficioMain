using Base.Contracts;

namespace Efficio.DAL.DTO.Security;

public class RolePermission : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    // Navigation
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
}