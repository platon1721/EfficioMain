using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IUserDepartmentRoleRepository : IBaseRepository<UserDepartmentRole>, IUserDepartmentRoleRepositoryCustom
{
}

public interface IUserDepartmentRoleRepositoryCustom
{
    Task<IEnumerable<UserDepartmentRole>> GetByUserAsync(Guid userId);
    Task<IEnumerable<UserDepartmentRole>> GetByDepartmentAsync(Guid departmentId);
    Task<IEnumerable<UserDepartmentRole>> GetByRoleAsync(Guid roleId);
    Task<UserDepartmentRole?> FindByUserAndDepartmentAsync(Guid userId, Guid departmentId);
    Task<bool> HasRoleInDepartmentAsync(Guid userId, Guid departmentId);
    Task<IEnumerable<UserDepartmentRole>> GetByUserInTenantAsync(Guid userId, Guid tenantRootDepartmentId);
}