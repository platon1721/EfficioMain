using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;

public interface IRoleRepository : IBaseRepository<Role>, IRoleRepositoryCustom
{
}

public interface IRoleRepositoryCustom
{ 
    Task<IEnumerable<Role>> GetByDepartmentAsync(Guid departmentId);
    Task<Role?> FindByNameAsync(Guid departmentId, string name);
    Task<Role?> FindWithPermissionsAsync(Guid roleId);
    Task<bool> NameExistsAsync(Guid departmentId, string name, Guid? excludeId = null);
}