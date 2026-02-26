using Base.DAL.Contracts;
using Efficio.DAL.DTO.Tenants;

namespace Efficio.DAL.Contracts;

public interface IUserTenantMembershipRepository : IBaseRepository<UserTenantMembership>, IUserTenantMembershipRepositoryCustom
{
}

public interface IUserTenantMembershipRepositoryCustom
{ 
    Task<IEnumerable<UserTenantMembership>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserTenantMembership>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<UserTenantMembership?> FindByUserAndTenantAsync(Guid userId, Guid tenantRootDepartmentId);
    Task<IEnumerable<UserTenantMembership>> GetActiveUsersByTenantAsync(Guid tenantRootDepartmentId);
    Task<bool> IsMemberAsync(Guid userId, Guid tenantRootDepartmentId);
}