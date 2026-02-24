using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IDepartmentRepository : IBaseRepository<Department>, IDepartmentRepositoryCustom
{
}

public interface IDepartmentRepositoryCustom
{
    Task<Department?> GetWithDepartmentTypeAsync(Guid id);
    Task<Department?> GetWithChildrenAsync(Guid id);
    Task<Department?> GetWithParentsAsync(Guid id);
    Task<Department?> GetWithAllRelationsAsync(Guid id);
    Task<IEnumerable<Department>> GetByDepartmentTypeAsync(Guid departmentTypeId);
    Task<IEnumerable<Department>> GetRootDepartmentsAsync();
}