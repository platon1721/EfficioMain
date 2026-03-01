using Base.BLL.Contracts;
using Efficio.BLL.DTO.Security;

namespace Efficio.BLL.Contracts;

public interface IModuleService : IBaseService<Module>, IModuleServiceCustom
{
}

public interface IModuleServiceCustom
{
    Task<Module?> FindByCodeAsync(string code);
    Task<IEnumerable<Module>> GetActiveModulesAsync();
    Task<Module?> GetMainModuleAsync();
}