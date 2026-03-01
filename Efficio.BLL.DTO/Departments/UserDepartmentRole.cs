using Base.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.DTO.Departments;

public class UserDepartmentRole : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    // Populated when loaded with navigation
    public Role? Role { get; set; }
    public Department? Department { get; set; }
}