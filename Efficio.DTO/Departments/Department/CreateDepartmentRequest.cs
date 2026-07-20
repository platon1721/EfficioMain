using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Departments.Department;

public class CreateDepartmentRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public Guid DepartmentTypeId { get; set; }
}