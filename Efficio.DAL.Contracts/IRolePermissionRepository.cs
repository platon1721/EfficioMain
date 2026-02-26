using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;

public interface IRolePermissionRepository : IBaseRepository<RolePermission>, IRolePermissionRepositoryCustom
{
}

public interface IRolePermissionRepositoryCustom
{
    Task<IEnumerable<RolePermission>> GetByRoleAsync(Guid roleId);
    Task<IEnumerable<RolePermission>> GetByPermissionAsync(Guid permissionId);
    Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId);
    Task<RolePermission?> FindByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
    Task RemoveByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
}
