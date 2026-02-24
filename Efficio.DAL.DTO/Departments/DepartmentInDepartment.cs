using Base.Contracts;

namespace Efficio.DAL.DTO.Departments;

public class DepartmentInDepartment : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid ParentDepartmentId { get; set; }
    public Guid ChildDepartmentId { get; set; }
    
    // Navigation
    public Department? ParentDepartment { get; set; }
    public Department? ChildDepartment { get; set; }
}