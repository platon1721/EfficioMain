using System.ComponentModel.DataAnnotations;
using Base.Domain;
using Efficio.Domain.Departments;

namespace Efficio.Domain.Security;

public class Role: BaseSoftDeleteDepartmentEntity
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public Department? Department { get; set; }
    public ICollection<RolePermission>? RolePermissions { get; set; } =  new List<RolePermission>();
}