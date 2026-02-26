using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Repositories;

public class RolePermissionRepository : BaseRepository<DalDto.RolePermission, Dom.RolePermission>, IRolePermissionRepository
{
    public RolePermissionRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new RolePermissionMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.RolePermission>> GetByRoleAsync(Guid roleId)
    {
        var entities = await GetQuery()
            .Include(rp => rp.Permission)
                .ThenInclude(p => p!.Module)
            .Where(rp => rp.RoleId == roleId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.RolePermission>> GetByPermissionAsync(Guid permissionId)
    {
        var entities = await GetQuery()
            .Include(rp => rp.Role)
            .Where(rp => rp.PermissionId == permissionId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> HasPermissionAsync(Guid roleId, Guid permissionId)
    {
        return await GetQuery()
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
    }

    public async Task<DalDto.RolePermission?> FindByRoleAndPermissionAsync(Guid roleId, Guid permissionId)
    {
        var entity = await GetQuery()
            .Include(rp => rp.Role)
            .Include(rp => rp.Permission)
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        return Mapper.Map(entity);
    }

    public async Task RemoveByRoleAndPermissionAsync(Guid roleId, Guid permissionId)
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
        if (entity != null)
        {
            RepositoryDbSet.Remove(entity);
        }
    }
}