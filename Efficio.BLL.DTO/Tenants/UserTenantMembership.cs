using Base.Contracts;

namespace Efficio.BLL.DTO.Tenants;

public class UserTenantMembership : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid UserId { get; set; }
    public UserMembershipStatus Status { get; set; }
}