using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IDepartmentInDepartmentRepository : IBaseRepository<DepartmentInDepartment>, IDepartmentInDepartmentRepositoryCustom
{
}

public interface IDepartmentInDepartmentRepositoryCustom
{
    Task<IEnumerable<DepartmentInDepartment>> GetByParentIdAsync(Guid parentDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetByChildIdAsync(Guid childDepartmentId);
    Task<DepartmentInDepartment?> FindByParentAndChildAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<bool> RelationExistsAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetWithDepartmentsAsync();
}