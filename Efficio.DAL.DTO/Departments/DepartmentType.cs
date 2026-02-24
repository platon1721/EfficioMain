using Base.Contracts;

namespace Efficio.DAL.DTO.Departments;

public class DepartmentType : IDomainId
{
    public Guid Id { get; set; }
    public Guid TenantRootDepartmentId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
}