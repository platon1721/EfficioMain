using Base.Domain;

namespace Efficio.Domain.Security;

public class RolePermission: BaseAuditableDepartmentEntity
{
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
}