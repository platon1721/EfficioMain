using Base.Contracts;

namespace Efficio.BLL.DTO.Departments;

public class DepartmentInDepartment : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ParentDepartmentId { get; set; }
    public Guid ChildDepartmentId { get; set; }

    // Populated when loaded with navigation
    public Department? ParentDepartment { get; set; }
    public Department? ChildDepartment { get; set; }
}