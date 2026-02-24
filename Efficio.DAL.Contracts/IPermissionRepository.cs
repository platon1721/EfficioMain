using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;

public interface IPermissionRepository : IBaseRepository<Permission>, IPermissionRepositoryCustom
{
}

public interface IPermissionRepositoryCustom
{
    Task<Permission?> FindByKeyAsync(string key);
    Task<IEnumerable<Permission>> GetByModuleIdAsync(Guid moduleId);
    Task<IEnumerable<Permission>> GetActivePermissionsAsync();
    Task<IEnumerable<Permission>> GetWithModuleAsync();
}