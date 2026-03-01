using Efficio.BLL.DTO.Departments;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.Contracts;

/// <summary>
/// Cross-cutting service for user access management.
/// Handles role assignment, permission checks, and department access.
/// </summary>
public interface IUserAccessService
{
    // Role assignment
    Task<UserDepartmentRole?> AssignRoleAsync(Guid userId, Guid departmentId, Guid roleId);
    Task<bool> RemoveRoleAsync(Guid userId, Guid departmentId);
    Task<UserDepartmentRole?> GetUserRoleInDepartmentAsync(Guid userId, Guid departmentId);
    Task<IEnumerable<UserDepartmentRole>> GetUserRolesInTenantAsync(Guid userId, Guid tenantRootDepartmentId);

    // Permission checks
    Task<bool> HasPermissionAsync(Guid userId, Guid departmentId, string permissionKey);
    Task<IEnumerable<string>> GetUserPermissionKeysAsync(Guid userId, Guid departmentId);
    Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, Guid departmentId);

    // Access validation
    Task<bool> CanAccessDepartmentAsync(Guid userId, Guid departmentId);
    Task<bool> CanAccessTenantAsync(Guid userId, Guid tenantRootDepartmentId);
}