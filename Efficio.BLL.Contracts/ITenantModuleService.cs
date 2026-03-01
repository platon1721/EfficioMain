using Base.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;

namespace Efficio.BLL.Contracts;

public interface ITenantModuleService : IBaseService<TenantModule>, ITenantModuleServiceCustom
{
}

public interface ITenantModuleServiceCustom
{
    Task<IEnumerable<TenantModule>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<IEnumerable<TenantModule>> GetActiveModulesForTenantAsync(Guid tenantRootDepartmentId);
    Task<TenantModule?> FindByTenantAndModuleAsync(Guid tenantRootDepartmentId, Guid moduleId);
    Task<bool> HasModuleAsync(Guid tenantRootDepartmentId, Guid moduleId);
}