using Base.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;

namespace Efficio.BLL.Contracts;

public interface IUserTenantMembershipService : IBaseService<UserTenantMembership>, IUserTenantMembershipServiceCustom
{
}

public interface IUserTenantMembershipServiceCustom
{
    Task<IEnumerable<UserTenantMembership>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserTenantMembership>> GetByTenantAsync(Guid tenantRootDepartmentId);
    Task<UserTenantMembership?> FindByUserAndTenantAsync(Guid userId, Guid tenantRootDepartmentId);
    Task<IEnumerable<UserTenantMembership>> GetActiveUsersByTenantAsync(Guid tenantRootDepartmentId);
    Task<bool> IsMemberAsync(Guid userId, Guid tenantRootDepartmentId);

    // Business logic
    Task<UserTenantMembership?> InviteUserAsync(Guid userId, Guid tenantRootDepartmentId);
    Task<bool> ActivateMembershipAsync(Guid userId, Guid tenantRootDepartmentId);
    Task<bool> SuspendMembershipAsync(Guid userId, Guid tenantRootDepartmentId);
    Task<bool> RemoveMembershipAsync(Guid userId, Guid tenantRootDepartmentId);
}