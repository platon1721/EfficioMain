using Base.BLL.Contracts;
using Efficio.BLL.DTO.Departments;

namespace Efficio.BLL.Contracts;

public interface IDepartmentService : IBaseService<Department>, IDepartmentServiceCustom
{
}

public interface IDepartmentServiceCustom
{
    Task<Department?> FindWithTypeAsync(Guid id);
    Task<Department?> FindWithHierarchyAsync(Guid id);
    Task<IEnumerable<Department>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<IEnumerable<Department>> GetByTypeAsync(Guid departmentTypeId);
    Task<IEnumerable<Department>> GetRootDepartmentsAsync(Guid tenantRootDepartmentId);
}