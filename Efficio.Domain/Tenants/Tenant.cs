using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace Efficio.Domain.Tenants;

public class Tenant : BaseSoftDeleteEntity
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    [Required]
    [MaxLength(10)]
    public string Code { get; set; } = default!;
    
    public string DefaultTimeZone { get; private set; } = "Europe/Tallinn";
    
    public string DefaultLocale { get; private set; } = "et-EE";
    
    public Guid RootDepartmentId { get; private set; }

    public TenantStatus Status { get; set; } = TenantStatus.Active;
    
    
    public Tenant(string name, string code, string defaultTimeZone = "Europe/Tallinn", string defaultLocale = "et-EE")
    {
        Name = name.Trim();
        Code = code.Trim().ToLowerInvariant();
        DefaultTimeZone = defaultTimeZone;
        DefaultLocale = defaultLocale;
    }
    
    public void SetRootDepartment(Guid rootDepartmentId)
    {
        if (rootDepartmentId == Guid.Empty) throw new ArgumentException("RootDepartmentId is empty");
        RootDepartmentId = rootDepartmentId;
    }
}