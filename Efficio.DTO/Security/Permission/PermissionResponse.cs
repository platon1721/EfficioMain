namespace Efficio.DTO.Security.Permission;

public class PermissionResponse
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string Key { get; set; } = default!;
    public string? Name { get; set; }
    public bool IsActive { get; set; }
}