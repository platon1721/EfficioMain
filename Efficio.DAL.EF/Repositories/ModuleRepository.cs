using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Repositories;

public class ModuleRepository : BaseRepository<DalDto.Module, Dom.Module>, IModuleRepository
{
    public ModuleRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new ModuleMapper(), userContext)
    {
    }

    public async Task<DalDto.Module?> FindByCodeAsync(string code)
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.Code == code);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.Module>> GetActiveModulesAsync()
    {
        var entities = await RepositoryDbSet
            .Where(m => m.IsActive)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<DalDto.Module?> GetMainModuleAsync()
    {
        var entity = await RepositoryDbSet
            .FirstOrDefaultAsync(m => m.IsMain);
        return Mapper.Map(entity);
    }
}