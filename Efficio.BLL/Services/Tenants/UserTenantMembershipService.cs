using Base.BLL;
using Efficio.BLL.Contracts;
using Efficio.BLL.DTO.Tenants;
using Efficio.BLL.Mappers.Tenants;
using Efficio.DAL.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Services;

public class UserTenantMembershipService
    : BaseService<UserTenantMembership, DalDto.UserTenantMembership, IUserTenantMembershipRepository>,
      IUserTenantMembershipService
{
    public UserTenantMembershipService(IUserTenantMembershipRepository repository)
        : base(repository, new UserTenantMembershipMapper())
    {
    }

    public async Task<IEnumerable<UserTenantMembership>> GetByUserIdAsync(Guid userId)
    {
        return (await Repository.GetByUserIdAsync(userId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<UserTenantMembership>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<UserTenantMembership?> FindByUserAndTenantAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return Mapper.Map(await Repository.FindByUserAndTenantAsync(userId, tenantRootDepartmentId));
    }

    public async Task<IEnumerable<UserTenantMembership>> GetActiveUsersByTenantAsync(Guid tenantRootDepartmentId)
    {
        return (await Repository.GetActiveUsersByTenantAsync(tenantRootDepartmentId))
            .Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> IsMemberAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await Repository.IsMemberAsync(userId, tenantRootDepartmentId);
    }

    public async Task<UserTenantMembership?> InviteUserAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        // Idempotent — if already a member, return existing
        var existing = await Repository.FindByUserAndTenantAsync(userId, tenantRootDepartmentId);
        if (existing != null) return Mapper.Map(existing);

        var membership = new DalDto.UserTenantMembership
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TenantRootDepartmentId = tenantRootDepartmentId,
            Status = DalDto.UserMembershipStatus.Invited
        };
        Repository.Add(membership);
        return Mapper.Map(membership);
    }

    public async Task<bool> ActivateMembershipAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await SetStatusAsync(userId, tenantRootDepartmentId, DalDto.UserMembershipStatus.Active);
    }

    public async Task<bool> SuspendMembershipAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await SetStatusAsync(userId, tenantRootDepartmentId, DalDto.UserMembershipStatus.Suspended);
    }

    public async Task<bool> RemoveMembershipAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await SetStatusAsync(userId, tenantRootDepartmentId, DalDto.UserMembershipStatus.Removed);
    }

    private async Task<bool> SetStatusAsync(Guid userId, Guid tenantRootDepartmentId, DalDto.UserMembershipStatus status)
    {
        var entity = await Repository.FindByUserAndTenantAsync(userId, tenantRootDepartmentId);
        if (entity == null) return false;

        entity.Status = status;
        Repository.Update(entity);
        return true;
    }
}