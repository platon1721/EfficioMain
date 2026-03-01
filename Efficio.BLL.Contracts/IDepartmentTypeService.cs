using Base.BLL.Contracts;
using Efficio.BLL.DTO.Departments;

namespace Efficio.BLL.Contracts;

public interface IDepartmentTypeService : IBaseService<DepartmentType>, IDepartmentTypeServiceCustom
{
}

public interface IDepartmentTypeServiceCustom
{
    Task<DepartmentType?> FindByNameAsync(string name);
    Task<bool> NameExistsAsync(string name, Guid? excludeId = null);
    Task<IEnumerable<DepartmentType>> GetByTenantAsync(Guid tenantRootDepartmentId);
}