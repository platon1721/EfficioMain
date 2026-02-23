using Base.Domain;

namespace Efficio.Domain.Departments;

public class DepartmentInDepartment: BaseAuditableTenantEntity
{
    public Guid ParentDepartmentId { get; set; }
    public Guid ChildDepartmentId { get; set; }

    public Department ParentDepartment { get; set; } = default;
    public Department ChildDepartment { get; set; }  = default;
}