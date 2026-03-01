using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;
using Efficio.BLL.Mappers.Tenants;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Services;

public class TenantModuleService
    : BaseService<TenantModule, DalDto.TenantModule, ITenantModuleRepository>,
        ITenantModuleService
{
    public TenantModuleService(ITenantModuleRepository repository)
        : base(repository, new TenantModuleMapper())
    {
    }

    public async Task<IEnumerable<TenantModule>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<TenantModule>> GetActiveModulesForTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetActiveModulesForTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<TenantModule?> FindByTenantAndModuleAsync(Guid tenantRootDepartmentId, Guid moduleId)
    {
        return Mapper.Map(await Repository.FindByTenantAndModuleAsync(tenantRootDepartmentId, moduleId));
    }

    public async Task<bool> HasModuleAsync(Guid tenantRootDepartmentId, Guid moduleId)
    {
        return await Repository.HasModuleAsync(tenantRootDepartmentId, moduleId);
    }
}