using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Security;
using Efficio.BLL.Mappers.Security;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Services;

public class RoleService
    : BaseService<Role, DalDto.Role, IRoleRepository>,
      IRoleService
{
    private readonly IRolePermissionRepository _rolePermissionRepository;

    public RoleService(IRoleRepository repository, IRolePermissionRepository rolePermissionRepository)
        : base(repository, new RoleMapper())
    {
        _rolePermissionRepository = rolePermissionRepository;
    }

    public async Task<IEnumerable<Role>> GetByDepartmentAsync(Guid departmentId)
    {
        return (await Repository.GetByDepartmentAsync(departmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<Role?> FindByNameAsync(Guid departmentId, string name)
    {
        return Mapper.Map(await Repository.FindByNameAsync(departmentId, name));
    }

    public async Task<Role?> FindWithPermissionsAsync(Guid roleId)
    {
        return Mapper.Map(await Repository.FindWithPermissionsAsync(roleId));
    }

    public async Task<bool> NameExistsAsync(Guid departmentId, string name, Guid? excludeId = null)
    {
        return await Repository.NameExistsAsync(departmentId, name, excludeId);
    }

    public async Task<Role?> AssignPermissionAsync(Guid roleId, Guid permissionId)
    {
        // Idempotent — don't add if already exists
        var alreadyHas = await _rolePermissionRepository.HasPermissionAsync(roleId, permissionId);
        if (!alreadyHas)
        {
            var role = await Repository.FindAsync(roleId);
            if (role == null) return null;

            var rolePermission = new DalDto.RolePermission
            {
                Id = Guid.NewGuid(),
                RoleId = roleId,
                PermissionId = permissionId,
                TenantRootDepartmentId = role.TenantRootDepartmentId,
                DepartmentId = role.DepartmentId
            };
            _rolePermissionRepository.Add(rolePermission);
        }

        return Mapper.Map(await Repository.FindWithPermissionsAsync(roleId));
    }

    public async Task<bool> RemovePermissionAsync(Guid roleId, Guid permissionId)
    {
        var exists = await _rolePermissionRepository.HasPermissionAsync(roleId, permissionId);
        if (!exists) return false;

        await _rolePermissionRepository.RemoveByRoleAndPermissionAsync(roleId, permissionId);
        return true;
    }

    public async Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId)
    {
        return await _rolePermissionRepository.HasPermissionAsync(roleId, permissionId);
    }
}