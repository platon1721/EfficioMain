using System.ComponentModel.DataAnnotations;

namespace Efficio.DTO.Departments.DepartmentHierarchy;

public class CreateDepartmentLinkRequest
{
    [Required]
    public Guid ParentDepartmentId { get; set; }

    [Required]
    public Guid ChildDepartmentId { get; set; }
}