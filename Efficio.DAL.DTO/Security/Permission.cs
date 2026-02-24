using Base.Contracts;

namespace Efficio.DAL.DTO.Security;

public class Permission : IDomainId
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public string Key { get; set; } = default!;
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    
    public Module? Module { get; set; }
    
}