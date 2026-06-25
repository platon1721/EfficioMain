using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Tenants.Tenant;

public class CreateTenantRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = default!;

    [MaxLength(50)]
    public string DefaultTimeZone { get; set; } = "Europe/Tallinn";

    [MaxLength(10)]
    public string DefaultLocale { get; set; } = "et-EE";
}