using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Departments;
using Efficio.BLL.Mappers.Departments;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Services;

public class DepartmentInDepartmentService
    : BaseService<DepartmentInDepartment, DalDto.DepartmentInDepartment, IDepartmentInDepartmentRepository>,
        IDepartmentInDepartmentService
{
    public DepartmentInDepartmentService(IDepartmentInDepartmentRepository repository)
        : base(repository, new DepartmentInDepartmentMapper())
    {
    }

    public async Task<IEnumerable<DepartmentInDepartment>> GetChildrenAsync(Guid parentDepartmentId)
    {
        return (await Repository.GetChildrenAsync(parentDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DepartmentInDepartment>> GetParentsAsync(Guid childDepartmentId)
    {
        return (await Repository.GetParentsAsync(childDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> LinkExistsAsync(Guid parentDepartmentId, Guid childDepartmentId)
    {
        return await Repository.LinkExistsAsync(parentDepartmentId, childDepartmentId);
    }

    public async Task<DepartmentInDepartment?> FindLinkAsync(Guid parentDepartmentId, Guid childDepartmentId)
    {
        return Mapper.Map(await Repository.FindLinkAsync(parentDepartmentId, childDepartmentId));
    }

    public async Task<IEnumerable<DepartmentInDepartment>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }
}