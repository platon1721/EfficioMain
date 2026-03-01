using Base.BLL.Contracts;

namespace Efficio.BLL.Contracts;

public interface IEfficioBLL : IBaseBLL
{
    // Departments
    IDepartmentService DepartmentService { get; }
    IDepartmentTypeService DepartmentTypeService { get; }
    IDepartmentInDepartmentService DepartmentInDepartmentService { get; }

    // Security
    IModuleService ModuleService { get; }
    IPermissionService PermissionService { get; }
    IRoleService RoleService { get; }
    IRolePermissionService RolePermissionService { get; }

    // Tenants
    ITenantService TenantService { get; }
    ITenantModuleService TenantModuleService { get; }
    IUserTenantMembershipService UserTenantMembershipService { get; }

    // Cross-cutting business logic
    IUserAccessService UserAccessService { get; }
}