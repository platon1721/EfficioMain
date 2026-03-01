using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Security;
using Efficio.BLL.Mappers.Security;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Services;

public class PermissionService
    : BaseService<Permission, DalDto.Permission, IPermissionRepository>,
        IPermissionService
{
    public PermissionService(IPermissionRepository repository)
        : base(repository, new PermissionMapper())
    {
    }

    public async Task<Permission?> FindByKeyAsync(string key)
    {
        return Mapper.Map(await Repository.FindByKeyAsync(key));
    }

    public async Task<IEnumerable<Permission>> GetByModuleIdAsync(Guid moduleId)
    {
        return (await Repository.GetByModuleIdAsync(moduleId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<Permission>> GetActivePermissionsAsync()
    {
        return (await Repository.GetActivePermissionsAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<Permission>> GetWithModuleAsync()
    {
        return (await Repository.GetWithModuleAsync())
            .Select(e => Mapper.Map(e)!);
    }
}