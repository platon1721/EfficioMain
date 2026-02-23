using Base.Domain;
using Efficio.Domain.Identity;
using Efficio.Domain.Security;

namespace Efficio.Domain.Departments;

public class UserDepartmentRole: BaseSoftDeleteDepartmentEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    public Role? Role { get; set; }
    public AppUser? User { get; set; }
    public Department? Department { get; set; }
}