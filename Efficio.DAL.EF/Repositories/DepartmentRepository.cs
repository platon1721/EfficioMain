using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Repositories;

public class DepartmentRepository : BaseRepository<DalDto.Department, Dom.Department>, IDepartmentRepository
{
    public DepartmentRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new DepartmentMapper(), userContext)
    {
    }

    public async Task<DalDto.Department?> FindWithTypeAsync(Guid id)
    {
        var entity = await RepositoryDbSet
            .Include(d => d.DepartmentType)
            .FirstOrDefaultAsync(d => d.Id == id);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.Department>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(d => d.DepartmentType)
            .Where(d => d.TenantRootDepartmentId == tenantRootDepartmentId)
            .OrderBy(d => d.Name)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.Department>> GetByTypeAsync(Guid departmentTypeId)
    {
        var entities = await RepositoryDbSet
            .Include(d => d.DepartmentType)
            .Where(d => d.DepartmentTypeId == departmentTypeId)
            .OrderBy(d => d.Name)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<DalDto.Department?> FindWithHierarchyAsync(Guid id)
    {
        var entity = await RepositoryDbSet
            .Include(d => d.DepartmentType)
            .Include(d => d.ParentDepartmentLinks!)
                .ThenInclude(l => l.ParentDepartment)
            .Include(d => d.ChildDepartmentLinks!)
                .ThenInclude(l => l.ChildDepartment)
            .FirstOrDefaultAsync(d => d.Id == id);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.Department>> GetRootDepartmentsAsync(Guid tenantRootDepartmentId)
    {
        var departmentsWithParents = RepositoryDbSet
            .SelectMany(d => d.ParentDepartmentLinks!)
            .Select(l => l.ChildDepartmentId);

        var entities = await RepositoryDbSet
            .Include(d => d.DepartmentType)
            .Where(d => d.TenantRootDepartmentId == tenantRootDepartmentId &&
                       !departmentsWithParents.Contains(d.Id))
            .OrderBy(d => d.Name)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public Task<DalDto.Department?> GetWithDepartmentTypeAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DalDto.Department?> GetWithChildrenAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DalDto.Department?> GetWithParentsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DalDto.Department?> GetWithAllRelationsAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DalDto.Department>> GetByDepartmentTypeAsync(Guid departmentTypeId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DalDto.Department>> GetRootDepartmentsAsync()
    {
        throw new NotImplementedException();
    }
}