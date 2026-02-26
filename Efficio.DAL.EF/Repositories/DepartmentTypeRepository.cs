using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Repositories;


public class DepartmentTypeRepository : BaseRepository<DalDto.DepartmentType, Dom.DepartmentType>, IDepartmentTypeRepository
{
    public DepartmentTypeRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new DepartmentTypeMapper(), userContext)
    {
    }

    public async Task<DalDto.DepartmentType?> FindByNameAsync(Guid tenantRootDepartmentId, string name)
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(dt => dt.TenantRootDepartmentId == tenantRootDepartmentId && 
                                       dt.Name == name);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.DepartmentType>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Where(dt => dt.TenantRootDepartmentId == tenantRootDepartmentId)
            .OrderBy(dt => dt.Name)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> NameExistsAsync(Guid tenantRootDepartmentId, string name, Guid? excludeId = null)
    {
        return await RepositoryDbSet
            .AnyAsync(dt => dt.TenantRootDepartmentId == tenantRootDepartmentId && 
                            dt.Name == name &&
                            (!excludeId.HasValue || dt.Id != excludeId.Value));
    }

    public Task<DalDto.DepartmentType?> FindByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<bool> NameExistsAsync(string name)
    {
        throw new NotImplementedException();
    }
}