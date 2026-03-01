using Base.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;

namespace Efficio.BLL.Contracts;

public interface ITenantService : IBaseService<Tenant>, ITenantServiceCustom
{
}

public interface ITenantServiceCustom
{
    Task<Tenant?> FindByCodeAsync(string code);
    Task<Tenant?> FindByRootDepartmentIdAsync(Guid rootDepartmentId);
    Task<IEnumerable<Tenant>> GetActiveTenantsAsync();
    Task<bool> CodeExistsAsync(string code, Guid? excludeId = null);
}