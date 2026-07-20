namespace Efficio.DTO.Departments.DepartmentHierarchy;

public class DepartmentLinkResponse
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ParentDepartmentId { get; set; }
    public Guid ChildDepartmentId { get; set; }
    public string? ParentDepartmentName { get; set; }
    public string? ChildDepartmentName { get; set; }
}