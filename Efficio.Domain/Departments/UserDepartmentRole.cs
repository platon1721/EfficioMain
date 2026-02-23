using Base.Domain;

namespace Efficio.Domain.Departments;

public class UserDepartmentRole: BaseSoftDeleteDepartmentEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
    
    
}