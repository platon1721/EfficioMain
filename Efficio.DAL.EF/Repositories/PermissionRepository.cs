using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Repositories;

public class PermissionRepository : BaseRepository<DalDto.Permission, Dom.Permission>, IPermissionRepository
{
    public PermissionRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new PermissionMapper(), userContext)
    {
    }

    public async Task<DalDto.Permission?> FindByKeyAsync(string key)
    {
        var entity = await RepositoryDbSet
            .Include(p => p.Module)
            .FirstOrDefaultAsync(p => p.Key == key);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.Permission>> GetByModuleIdAsync(Guid moduleId)
    {
        var entities = await RepositoryDbSet
            .Include(p => p.Module)
            .Where(p => p.ModuleId == moduleId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.Permission>> GetActivePermissionsAsync()
    {
        var entities = await RepositoryDbSet
            .Include(p => p.Module)
            .Where(p => p.IsActive)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.Permission>> GetWithModuleAsync()
    {
        var entities = await RepositoryDbSet
            .Include(p => p.Module)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }
}