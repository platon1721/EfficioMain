using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Departments;
using Efficio.BLL.DTO.Security;
using Efficio.BLL.Mappers.Departments;
using Efficio.BLL.Mappers.Security;
using Efficio.DAL.Contracts;
using DalDepDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Services;

public class UserAccessService : IUserAccessService
{
    private readonly IUserDepartmentRoleRepository _userDepartmentRoleRepository;
    private readonly IRolePermissionRepository _rolePermissionRepository;
    private readonly IUserTenantMembershipRepository _userTenantMembershipRepository;
    private readonly UserDepartmentRoleMapper _udrMapper = new();
    private readonly PermissionMapper _permissionMapper = new();

    public UserAccessService(
        IUserDepartmentRoleRepository userDepartmentRoleRepository,
        IRolePermissionRepository rolePermissionRepository,
        IUserTenantMembershipRepository userTenantMembershipRepository)
    {
        _userDepartmentRoleRepository = userDepartmentRoleRepository;
        _rolePermissionRepository = rolePermissionRepository;
        _userTenantMembershipRepository = userTenantMembershipRepository;
    }

    public async Task<UserDepartmentRole?> AssignRoleAsync(Guid userId, Guid departmentId, Guid roleId)
    {
        // Remove existing role in this department first (one role per user per department)
        var existing = await _userDepartmentRoleRepository.FindByUserAndDepartmentAsync(userId, departmentId);
        if (existing != null)
        {
            _userDepartmentRoleRepository.Remove(existing);
        }

        var entity = new DalDepDto.UserDepartmentRole
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DepartmentId = departmentId,
            RoleId = roleId
        };
        _userDepartmentRoleRepository.Add(entity);
        return _udrMapper.Map(entity);
    }

    public async Task<bool> RemoveRoleAsync(Guid userId, Guid departmentId)
    {
        var existing = await _userDepartmentRoleRepository.FindByUserAndDepartmentAsync(userId, departmentId);
        if (existing == null) return false;

        _userDepartmentRoleRepository.Remove(existing);
        return true;
    }

    public async Task<UserDepartmentRole?> GetUserRoleInDepartmentAsync(Guid userId, Guid departmentId)
    {
        return _udrMapper.Map(
            await _userDepartmentRoleRepository.FindByUserAndDepartmentAsync(userId, departmentId));
    }

    public async Task<IEnumerable<UserDepartmentRole>> GetUserRolesInTenantAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return (await _userDepartmentRoleRepository.GetByUserInTenantAsync(userId, tenantRootDepartmentId))
            .Select(e => _udrMapper.Map(e)!);
    }

    public async Task<bool> HasPermissionAsync(Guid userId, Guid departmentId, string permissionKey)
    {
        var udr = await _userDepartmentRoleRepository.FindByUserAndDepartmentAsync(userId, departmentId);
        if (udr == null) return false;

        var rolePermissions = await _rolePermissionRepository.GetByRoleAsync(udr.RoleId);
        return rolePermissions.Any(rp => rp.Permission?.Key == permissionKey);
    }

    public async Task<IEnumerable<string>> GetUserPermissionKeysAsync(Guid userId, Guid departmentId)
    {
        var permissions = await GetUserPermissionsAsync(userId, departmentId);
        return permissions.Select(p => p.Key);
    }

    public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId, Guid departmentId)
    {
        var udr = await _userDepartmentRoleRepository.FindByUserAndDepartmentAsync(userId, departmentId);
        if (udr == null) return Enumerable.Empty<Permission>();

        var rolePermissions = await _rolePermissionRepository.GetByRoleAsync(udr.RoleId);
        return rolePermissions
            .Where(rp => rp.Permission != null)
            .Select(rp => _permissionMapper.Map(rp.Permission)!);
    }

    public async Task<bool> CanAccessDepartmentAsync(Guid userId, Guid departmentId)
    {
        return await _userDepartmentRoleRepository.HasRoleInDepartmentAsync(userId, departmentId);
    }

    public async Task<bool> CanAccessTenantAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await _userTenantMembershipRepository.IsMemberAsync(userId, tenantRootDepartmentId);
    }
}