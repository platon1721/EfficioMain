using Base.Contracts;

namespace Efficio.BLL.DTO.Tenants;

public class Tenant : IDomainId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string DefaultTimeZone { get; set; } = default!;
    public string DefaultLocale { get; set; } = default!;
    public Guid RootDepartmentId { get; set; }
    public TenantStatus Status { get; set; }
}