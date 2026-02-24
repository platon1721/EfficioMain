using Base.DAL.Contracts;
using Efficio.DAL.DTO.Security;

namespace Efficio.DAL.Contracts;


public interface IModuleRepository : IBaseRepository<Module>, IModuleRepositoryCustom
{
}

public interface IModuleRepositoryCustom
{
    Task<Module?> FindByCodeAsync(string code);
    Task<IEnumerable<Module>> GetActiveModulesAsync();
    Task<Module?> GetMainModuleAsync();
}