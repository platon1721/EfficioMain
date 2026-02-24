using Base.DAL.Contracts;
using Efficio.DAL.DTO.Departments;

namespace Efficio.DAL.Contracts;

public interface IUserDepartmentRoleRepository : IBaseRepository<UserDepartmentRole>, IUserDepartmentRoleRepositoryCustom
{
}

public interface IUserDepartmentRoleRepositoryCustom
{
    Task<IEnumerable<UserDepartmentRole>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserDepartmentRole>> GetByRoleIdAsync(Guid roleId);
    Task<IEnumerable<UserDepartmentRole>> GetByDepartmentIdAsync(Guid departmentId);
    Task<UserDepartmentRole?> FindByUserAndDepartmentAsync(Guid userId, Guid departmentId);
    Task<bool> UserHasRoleInDepartmentAsync(Guid userId, Guid departmentId);
    Task<IEnumerable<UserDepartmentRole>> GetWithRoleAndDepartmentAsync();
    Task<IEnumerable<string>> GetUserPermissionKeysAsync(Guid userId, Guid departmentId);
}