using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Tenants.UserTenantMembership;

public class InviteUserRequest
{
    [Required]
    public Guid UserId { get; set; }
}