using Base.DAL.Contracts;

namespace Efficio.DAL.Contracts;

public interface IEfficioUOW : IBaseUOW
{
    // Security (global)
    IModuleRepository ModuleRepository { get; }
    IPermissionRepository PermissionRepository { get; }
    
    // Tenants
    ITenantRepository TenantRepository { get; }
    ITenantModuleRepository TenantModuleRepository { get; }
    IUserTenantMembershipRepository UserTenantMembershipRepository { get; }
    
    // Departments
    IDepartmentRepository DepartmentRepository { get; }
    IDepartmentTypeRepository DepartmentTypeRepository { get; }
    IDepartmentInDepartmentRepository DepartmentInDepartmentRepository { get; }
    
    // Security (department-scoped)
    IRoleRepository RoleRepository { get; }
    IRolePermissionRepository RolePermissionRepository { get; }
    IUserDepartmentRoleRepository UserDepartmentRoleRepository { get; }
}