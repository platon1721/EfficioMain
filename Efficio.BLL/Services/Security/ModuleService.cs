using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Security;
using Efficio.BLL.Mappers.Security;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Services;

public class ModuleService
    : BaseService<Module, DalDto.Module, IModuleRepository>,
        IModuleService
{
    public ModuleService(IModuleRepository repository)
        : base(repository, new ModuleMapper())
    {
    }

    public async Task<Module?> FindByCodeAsync(string code)
    {
        return Mapper.Map(await Repository.FindByCodeAsync(code));
    }

    public async Task<IEnumerable<Module>> GetActiveModulesAsync()
    {
        return (await Repository.GetActiveModulesAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<Module?> GetMainModuleAsync()
    {
        return Mapper.Map(await Repository.GetMainModuleAsync());
    }
}