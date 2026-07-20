using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Departments.DepartmentType;

public class CreateDepartmentTypeRequest
{
    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }
}