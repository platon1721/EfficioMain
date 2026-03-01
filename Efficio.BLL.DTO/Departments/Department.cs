using Base.Contracts;

namespace Efficio.BLL.DTO.Departments;

public class Department : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public Guid DepartmentTypeId { get; set; }

    // Populated when loaded with type
    public DepartmentType? DepartmentType { get; set; }

    // Populated when loaded with hierarchy
    public ICollection<DepartmentInDepartment>? ParentLinks { get; set; }
    public ICollection<DepartmentInDepartment>? ChildLinks { get; set; }
}