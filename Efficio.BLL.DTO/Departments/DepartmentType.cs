using Base.Contracts;

namespace Efficio.BLL.DTO.Departments;

public class DepartmentType : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}