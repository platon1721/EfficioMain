using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Repositories;


public class TenantRepository : BaseRepository<DalDto.Tenant, Dom.Tenant>, ITenantRepository
{
    public TenantRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new TenantMapper(), userContext)
    {
    }

    public async Task<DalDto.Tenant?> FindByCodeAsync(string code)
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(t => t.Code == code);
        return Mapper.Map(entity);
    }

    public Task<IEnumerable<DalDto.Tenant>> GetByStatusAsync(DalDto.TenantStatus status)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CodeExistsAsync(string code)
    {
        throw new NotImplementedException();
    }

    public async Task<DalDto.Tenant?> FindByRootDepartmentIdAsync(Guid rootDepartmentId)
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(t => t.RootDepartmentId == rootDepartmentId);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.Tenant>> GetActiveTenantsAsync()
    {
        var entities = await RepositoryDbSet
            .Where(t => t.Status == Dom.TenantStatus.Active)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> CodeExistsAsync(string code, Guid? excludeId = null)
    {
        return await RepositoryDbSet
            .AnyAsync(t => t.Code == code && (!excludeId.HasValue || t.Id != excludeId.Value));
    }
}