using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Departments;
using Efficio.BLL.Mappers.Departments;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Services;

public class DepartmentTypeService
    : BaseService<DepartmentType, DalDto.DepartmentType, IDepartmentTypeRepository>,
        IDepartmentTypeService
{
    public DepartmentTypeService(IDepartmentTypeRepository repository)
        : base(repository, new DepartmentTypeMapper())
    {
    }

    public async Task<DepartmentType?> FindByNameAsync(string name)
    {
        return Mapper.Map(await Repository.FindByNameAsync(name));
    }

    public async Task<bool> NameExistsAsync(string name, Guid? excludeId = null)
    {
        return await Repository.NameExistsAsync(name, excludeId);
    }

    public async Task<IEnumerable<DepartmentType>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }
}