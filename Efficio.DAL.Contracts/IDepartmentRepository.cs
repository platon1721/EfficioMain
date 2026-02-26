using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IDepartmentRepository : IBaseRepository<Department>, IDepartmentRepositoryCustom
{
}

public interface IDepartmentRepositoryCustom
{
    Task<Department?> FindWithTypeAsync(Guid id);
    Task<Department?> FindWithHierarchyAsync(Guid id);
    Task<IEnumerable<Department>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<IEnumerable<Department>> GetByTypeAsync(Guid departmentTypeId);
    Task<IEnumerable<Department>> GetRootDepartmentsAsync(Guid tenantRootDepartmentId);
}