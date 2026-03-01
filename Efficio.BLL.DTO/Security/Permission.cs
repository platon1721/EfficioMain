using Base.Contracts;

namespace Efficio.BLL.DTO.Security;

public class Permission : IDomainId
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string Key { get; set; } = default!;
    public string? Name { get; set; }
    public bool IsActive { get; set; }

    // Populated when loaded with module
    public Module? Module { get; set; }
}