using Base.BLL.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.Contracts;

public interface IPermissionService : IBaseService<Permission>, IPermissionServiceCustom
{
}

public interface IPermissionServiceCustom
{
    Task<Permission?> FindByKeyAsync(string key);
    Task<IEnumerable<Permission>> GetByModuleIdAsync(Guid moduleId);
    Task<IEnumerable<Permission>> GetActivePermissionsAsync();
    Task<IEnumerable<Permission>> GetWithModuleAsync();
}