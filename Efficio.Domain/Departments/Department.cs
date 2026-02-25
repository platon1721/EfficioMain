using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace Efficio.Domain.Departments;

public class Department: BaseSoftDeleteTenantEntity
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;
    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public Guid DepartmentTypeId { get; set; } = default!;
    public DepartmentType? DepartmentType { get; set; }
    public ICollection<DepartmentInDepartment> ParentDepartmentLinks { get; set; } = new List<DepartmentInDepartment>();
    public ICollection<DepartmentInDepartment> ChildDepartmentLinks { get; set; } = new List<DepartmentInDepartment>();


}