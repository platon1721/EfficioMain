using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Repositories;

public class DepartmentInDepartmentRepository : BaseRepository<DalDto.DepartmentInDepartment, Dom.DepartmentInDepartment>, IDepartmentInDepartmentRepository
{
    public DepartmentInDepartmentRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new DepartmentInDepartmentMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.DepartmentInDepartment>> GetChildrenAsync(Guid parentDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(l => l.ChildDepartment)
            .Where(l => l.ParentDepartmentId == parentDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.DepartmentInDepartment>> GetParentsAsync(Guid childDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(l => l.ParentDepartment)
            .Where(l => l.ChildDepartmentId == childDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> LinkExistsAsync(Guid parentDepartmentId, Guid childDepartmentId)
    {
        return await RepositoryDbSet
            .AnyAsync(l => l.ParentDepartmentId == parentDepartmentId &&
                           l.ChildDepartmentId == childDepartmentId);
    }

    public async Task<DalDto.DepartmentInDepartment?> FindLinkAsync(Guid parentDepartmentId, Guid childDepartmentId)
    {
        var entity = await RepositoryDbSet
            .Include(l => l.ParentDepartment)
            .Include(l => l.ChildDepartment)
            .FirstOrDefaultAsync(l => l.ParentDepartmentId == parentDepartmentId &&
                                      l.ChildDepartmentId == childDepartmentId);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.DepartmentInDepartment>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(l => l.ParentDepartment)
            .Include(l => l.ChildDepartment)
            .Where(l => l.TenantRootDepartmentId == tenantRootDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }
}