using Base.BLL.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.Contracts;

public interface IRolePermissionService : IBaseService<RolePermission>, IRolePermissionServiceCustom
{
}

public interface IRolePermissionServiceCustom
{
    Task<IEnumerable<RolePermission>> GetByRoleAsync(Guid roleId);
    Task<IEnumerable<RolePermission>> GetByPermissionAsync(Guid permissionId);
    Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId);
    Task<RolePermission?> FindByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
    Task RemoveByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
}