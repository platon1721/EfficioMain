using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Security.Role;

public class AssignPermissionRequest
{
    [Required]
    public Guid PermissionId { get; set; }
}