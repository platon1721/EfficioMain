using Efficio.DTO.Security.Permission;

namespace Efficio.DTO.Security.Role;

public class RoleWithPermissionsResponse : RoleResponse
{
    public ICollection<PermissionResponse> Permissions { get; set; } = new List<PermissionResponse>();
}