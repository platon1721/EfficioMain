using Base.Contracts;

namespace Efficio.DAL.DTO.Tenants;

public class UserTenantMembership : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid UserId { get; set; }
    public UserMembershipStatus Status { get; set; }
}