using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;

public interface IRolePermissionRepository : IBaseRepository<RolePermission>, IRolePermissionRepositoryCustom
{
}

public interface IRolePermissionRepositoryCustom
{
    Task<IEnumerable<RolePermission>> GetByRoleIdAsync(Guid roleId);
    Task<IEnumerable<RolePermission>> GetByPermissionIdAsync(Guid permissionId);
    Task<RolePermission?> FindByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
    Task<bool> RoleHasPermissionAsync(Guid roleId, Guid permissionId);
    Task<IEnumerable<RolePermission>> GetWithRoleAndPermissionAsync();
    Task RemoveByRoleAndPermissionAsync(Guid roleId, Guid permissionId);
}