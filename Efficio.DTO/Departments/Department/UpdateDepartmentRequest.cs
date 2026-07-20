using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Departments.Department;

public class UpdateDepartmentRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    public Guid? DepartmentTypeId { get; set; }
}