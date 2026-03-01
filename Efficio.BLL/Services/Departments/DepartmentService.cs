using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Departments;
using Efficio.BLL.Mappers.Departments;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Services;

public class DepartmentService
    : BaseService<Department, DalDto.Department, IDepartmentRepository>,
        IDepartmentService
{
    public DepartmentService(IDepartmentRepository repository)
        : base(repository, new DepartmentMapper())
    {
    }

    public async Task<Department?> FindWithTypeAsync(Guid id)
    {
        return Mapper.Map(await Repository.FindWithTypeAsync(id));
    }

    public async Task<Department?> FindWithHierarchyAsync(Guid id)
    {
        return Mapper.Map(await Repository.FindWithHierarchyAsync(id));
    }

    public async Task<IEnumerable<Department>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<Department>> GetByTypeAsync(Guid departmentTypeId)
    {
        return (await Repository.GetByTypeAsync(departmentTypeId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<Department>> GetRootDepartmentsAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetRootDepartmentsAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }
}