using Base.Contracts;

namespace Efficio.DAL.DTO.Departments;

public class Department : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public Guid DepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid DepartmentTypeId { get; set; }
    
    // Navigation
    public DepartmentType? DepartmentType { get; set; }
    public ICollection<DepartmentInDepartment>? ParentDepartmentLinks { get; set; }
    public ICollection<DepartmentInDepartment>? ChildDepartmentLinks { get; set; }
}