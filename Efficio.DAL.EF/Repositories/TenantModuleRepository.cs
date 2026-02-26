using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Repositories;

public class TenantModuleRepository : BaseRepository<DalDto.TenantModule, Dom.TenantModule>, ITenantModuleRepository
{
    public TenantModuleRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new TenantModuleMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.TenantModule>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(tm => tm.Module)
            .Where(tm => tm.TenantRootDepartmentId == tenantRootDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public Task<IEnumerable<DalDto.TenantModule>> GetWithModuleAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> TenantHasModuleAsync(Guid tenantRootDepartmentId, Guid moduleId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DalDto.TenantModule>> GetActiveModulesForTenantAsync(Guid tenantRootDepartmentId)
    {
        var now = DateTime.UtcNow;
        var entities = await RepositoryDbSet
            .Include(tm => tm.Module)
            .Where(tm => tm.TenantRootDepartmentId == tenantRootDepartmentId && 
                        (tm.ExpiresAt == null || tm.ExpiresAt > now))
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> HasModuleAsync(Guid tenantRootDepartmentId, Guid moduleId)
    {
        var now = DateTime.UtcNow;
        return await RepositoryDbSet
            .AnyAsync(tm => tm.TenantRootDepartmentId == tenantRootDepartmentId && 
                           tm.ModuleId == moduleId &&
                           (tm.ExpiresAt == null || tm.ExpiresAt > now));
    }

    public async Task<DalDto.TenantModule?> FindByTenantAndModuleAsync(Guid tenantRootDepartmentId, Guid moduleId)
    {
        var entity = await RepositoryDbSet
            .Include(tm => tm.Module)
            .FirstOrDefaultAsync(tm => tm.TenantRootDepartmentId == tenantRootDepartmentId && 
                                       tm.ModuleId == moduleId);
        return Mapper.Map(entity);
    }
}