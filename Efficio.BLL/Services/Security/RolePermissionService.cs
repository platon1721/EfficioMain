using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Security;
using Efficio.BLL.Mappers.Security;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Services;

public class RolePermissionService
    : BaseService<RolePermission, DalDto.RolePermission, IRolePermissionRepository>,
        IRolePermissionService
{
    public RolePermissionService(IRolePermissionRepository repository)
        : base(repository, new RolePermissionMapper())
    {
    }

    public async Task<IEnumerable<RolePermission>> GetByRoleAsync(Guid roleId)
    {
        return (await Repository.GetByRoleAsync(roleId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<RolePermission>> GetByPermissionAsync(Guid permissionId)
    {
        return (await Repository.GetByPermissionAsync(permissionId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId)
    {
        return await Repository.HasPermissionAsync(roleId, permissionId);
    }

    public async Task<RolePermission?> FindByRoleAndPermissionAsync(Guid roleId, Guid permissionId)
    {
        return Mapper.Map(await Repository.FindByRoleAndPermissionAsync(roleId, permissionId));
    }

    public async Task RemoveByRoleAndPermissionAsync(Guid roleId, Guid permissionId)
    {
        await Repository.RemoveByRoleAndPermissionAsync(roleId, permissionId);
    }
}