using Base.Contracts;

namespace Efficio.BLL.DTO.Security;

public class Module : IDomainId
{
    public Guid Id { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsMain { get; set; }
    public bool IsActive { get; set; }
}