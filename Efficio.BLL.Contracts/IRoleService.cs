using Base.BLL.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.Contracts;

public interface IRoleService : IBaseService<Role>, IRoleServiceCustom
{
}

public interface IRoleServiceCustom
{
    Task<IEnumerable<Role>> GetByDepartmentAsync(Guid departmentId);
    Task<Role?> FindByNameAsync(Guid departmentId, string name);
    Task<Role?> FindWithPermissionsAsync(Guid roleId);
    Task<bool> NameExistsAsync(Guid departmentId, string name, Guid? excludeId = null);

    // Business logic
    Task<Role?> AssignPermissionAsync(Guid roleId, Guid permissionId);
    Task<bool> RemovePermissionAsync(Guid roleId, Guid permissionId);
    Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId);
}