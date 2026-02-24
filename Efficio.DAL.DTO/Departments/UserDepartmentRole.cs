using Base.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.DTO.Departments;

public class UserDepartmentRole : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    // Navigation
    public Role? Role { get; set; }
    public Department? Department { get; set; }
}