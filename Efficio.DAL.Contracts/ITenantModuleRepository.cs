using Base.DAL.Contracts;
using Efficio.DAL.DTO.Tenants;

namespace Efficio.DAL.Contracts;

public interface ITenantModuleRepository : IBaseRepository<TenantModule>, ITenantModuleRepositoryCustom
{
}

public interface ITenantModuleRepositoryCustom
{
    Task<IEnumerable<TenantModule>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<IEnumerable<TenantModule>> GetWithModuleAsync();
    Task<TenantModule?> FindByTenantAndModuleAsync(Guid tenantRootDepartmentId, Guid moduleId);
    Task<bool> TenantHasModuleAsync(Guid tenantRootDepartmentId, Guid moduleId);
    Task<IEnumerable<TenantModule>> GetActiveModulesForTenantAsync(Guid tenantRootDepartmentId);
}