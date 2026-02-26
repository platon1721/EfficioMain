using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IDepartmentInDepartmentRepository : IBaseRepository<DepartmentInDepartment>, IDepartmentInDepartmentRepositoryCustom
{
}

public interface IDepartmentInDepartmentRepositoryCustom
{
    Task<IEnumerable<DepartmentInDepartment>> GetChildrenAsync(Guid parentDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetParentsAsync(Guid childDepartmentId);
    Task<bool> LinkExistsAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<DepartmentInDepartment?> FindLinkAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetByTenantAsync(Guid tenantRootDepartmentId);
}