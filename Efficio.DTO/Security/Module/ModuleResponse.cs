namespace Efficio.DTO.Security.Module;

public class ModuleResponse
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsMain { get; set; }
    public bool IsActive { get; set; }
}