using Base.BLL.Contracts;
using Efficio.BLL.DTO.Departments;

namespace Efficio.BLL.Contracts;

public interface IDepartmentInDepartmentService : IBaseService<DepartmentInDepartment>, IDepartmentInDepartmentServiceCustom
{
}

public interface IDepartmentInDepartmentServiceCustom
{
    Task<IEnumerable<DepartmentInDepartment>> GetChildrenAsync(Guid parentDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetParentsAsync(Guid childDepartmentId);
    Task<bool> LinkExistsAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<DepartmentInDepartment?> FindLinkAsync(Guid parentDepartmentId, Guid childDepartmentId);
    Task<IEnumerable<DepartmentInDepartment>> GetByTenantAsync(Guid tenantRootDepartmentId);
}