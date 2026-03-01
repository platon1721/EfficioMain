using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;
using Efficio.BLL.Mappers.Tenants;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Services;

public class TenantService
    : BaseService<Tenant, DalDto.Tenant, ITenantRepository>,
        ITenantService
{
    public TenantService(ITenantRepository repository)
        : base(repository, new TenantMapper())
    {
    }

    public async Task<Tenant?> FindByCodeAsync(string code)
    {
        return Mapper.Map(await Repository.FindByCodeAsync(code));
    }

    public async Task<Tenant?> FindByRootDepartmentIdAsync(Guid rootDepartmentId)
    {
        return Mapper.Map(await Repository.FindByRootDepartmentIdAsync(rootDepartmentId));
    }

    public async Task<IEnumerable<Tenant>> GetActiveTenantsAsync()
    {
        return (await Repository.GetActiveTenantsAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> CodeExistsAsync(string code, Guid? excludeId = null)
    {
        return await Repository.CodeExistsAsync(code, excludeId);
    }
}