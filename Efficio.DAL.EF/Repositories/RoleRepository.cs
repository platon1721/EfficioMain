using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Repositories;

public class RoleRepository : BaseRepository<DalDto.Role, Dom.Role>, IRoleRepository
{
    public RoleRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new RoleMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.Role>> GetByDepartmentAsync(Guid departmentId)
    {
        var entities = await GetQuery()
            .Include(r => r.Department)
            .Where(r => r.DepartmentId == departmentId)
            .OrderBy(r => r.Name)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<DalDto.Role?> FindByNameAsync(Guid departmentId, string name)
    {
        var entity = await GetQuery()
            .Include(r => r.Department)
            .FirstOrDefaultAsync(r => r.DepartmentId == departmentId && r.Name == name);
        return Mapper.Map(entity);
    }

    public async Task<DalDto.Role?> FindWithPermissionsAsync(Guid roleId)
    {
        var entity = await GetQuery()
            .Include(r => r.Department)
            .Include(r => r.RolePermissions!)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.Id == roleId);
        return Mapper.Map(entity);
    }

    public async Task<bool> NameExistsAsync(Guid departmentId, string name, Guid? excludeId = null)
    {
        return await GetQuery()
            .AnyAsync(r => r.DepartmentId == departmentId &&
                           r.Name == name &&
                           (!excludeId.HasValue || r.Id != excludeId.Value));
    }
}