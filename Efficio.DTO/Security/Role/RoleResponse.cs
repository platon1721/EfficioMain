namespace Efficio.DTO.Security.Role;

public class RoleResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? DepartmentName { get; set; }
}