namespace Efficio.DTO.Departments.DepartmentType;

public class DepartmentTypeResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}