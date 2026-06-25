namespace Efficio.DTO.Tenants.UserTenantMembership;

public class UserTenantMembershipResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; } = default!;
}