namespace Efficio.DTO.Tenants.Tenant;

public class TenantResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!;
    public string DefaultTimeZone { get; set; } = default!;
    public string DefaultLocale { get; set; } = default!;
    public Guid RootDepartmentId { get; set; }
    public string Status { get; set; } = default!;
}