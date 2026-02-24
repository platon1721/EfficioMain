using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;

public interface IRoleRepository : IBaseRepository<Role>, IRoleRepositoryCustom
{
}

public interface IRoleRepositoryCustom
{
    Task<Role?> FindByNameAsync(string name);
    Task<Role?> GetWithPermissionsAsync(Guid id);
    Task<IEnumerable<Role>> GetWithPermissionsAsync();
    Task<IEnumerable<Role>> GetByDepartmentIdAsync(Guid departmentId);
    Task<bool> NameExistsInDepartmentAsync(string name, Guid departmentId);
}