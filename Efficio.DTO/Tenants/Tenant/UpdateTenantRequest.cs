using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Tenants.Tenant;

public class UpdateTenantRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(50)]
    public string? DefaultTimeZone { get; set; }

    [MaxLength(10)]
    public string? DefaultLocale { get; set; }
}