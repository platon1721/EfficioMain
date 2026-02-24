using Base.DAL.Contracts;
using Efficio.DAL.DTO.Tenants;

namespace Efficio.DAL.Contracts;

public interface ITenantRepository : IBaseRepository<Tenant>, ITenantRepositoryCustom
{
}

public interface ITenantRepositoryCustom
{
    Task<Tenant?> FindByCodeAsync(string code);
    Task<IEnumerable<Tenant>> GetByStatusAsync(TenantStatus status);
    Task<bool> CodeExistsAsync(string code);
}