namespace Efficio.DTO.Tenants.TenantModule;

public class TenantModuleResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ModuleId { get; set; }
    public string? ModuleName { get; set; }
    public string? ModuleCode { get; set; }
    public DateTime? ExpiresAt { get; set; }
}