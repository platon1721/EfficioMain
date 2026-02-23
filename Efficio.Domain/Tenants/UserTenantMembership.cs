using Base.Domain;
using Efficio.Domain.Identity;

namespace Efficio.Domain.Tenants;

public class UserTenantMembership : BaseSoftDeleteTenantEntity
{
    public Guid UserId { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    
    public UserMembershipStatus Status { get; set; } = UserMembershipStatus.Invited;
}