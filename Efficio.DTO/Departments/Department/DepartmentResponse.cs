namespace Efficio.DTO.Departments.Department;

public class DepartmentResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid DepartmentTypeId { get; set; }
    public string? DepartmentTypeName { get; set; }
}