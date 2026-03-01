using Efficio.BLL.Contracts;
using Efficio.BLL.Services;
using Efficio.DAL.Contracts;

namespace Efficio.BLL;

public class EfficioBLL : IEfficioBLL
{
    private readonly IEfficioUOW _uow;

    // Lazy init — only create service instances when first accessed
    private IDepartmentService? _departmentService;
    private IDepartmentTypeService? _departmentTypeService;
    private IDepartmentInDepartmentService? _departmentInDepartmentService;
    private IModuleService? _moduleService;
    private IPermissionService? _permissionService;
    private IRoleService? _roleService;
    private IRolePermissionService? _rolePermissionService;
    private ITenantService? _tenantService;
    private ITenantModuleService? _tenantModuleService;
    private IUserTenantMembershipService? _userTenantMembershipService;
    private IUserAccessService? _userAccessService;

    public EfficioBLL(IEfficioUOW uow)
    {
        _uow = uow;
    }

    public IDepartmentService DepartmentService =>
        _departmentService ??= new DepartmentService(_uow.DepartmentRepository);

    public IDepartmentTypeService DepartmentTypeService =>
        _departmentTypeService ??= new DepartmentTypeService(_uow.DepartmentTypeRepository);

    public IDepartmentInDepartmentService DepartmentInDepartmentService =>
        _departmentInDepartmentService ??= new DepartmentInDepartmentService(_uow.DepartmentInDepartmentRepository);

    public IModuleService ModuleService =>
        _moduleService ??= new ModuleService(_uow.ModuleRepository);

    public IPermissionService PermissionService =>
        _permissionService ??= new PermissionService(_uow.PermissionRepository);

    public IRoleService RoleService =>
        _roleService ??= new RoleService(_uow.RoleRepository, _uow.RolePermissionRepository);

    public IRolePermissionService RolePermissionService =>
        _rolePermissionService ??= new RolePermissionService(_uow.RolePermissionRepository);

    public ITenantService TenantService =>
        _tenantService ??= new TenantService(_uow.TenantRepository);

    public ITenantModuleService TenantModuleService =>
        _tenantModuleService ??= new TenantModuleService(_uow.TenantModuleRepository);

    public IUserTenantMembershipService UserTenantMembershipService =>
        _userTenantMembershipService ??= new UserTenantMembershipService(_uow.UserTenantMembershipRepository);

    public IUserAccessService UserAccessService =>
        _userAccessService ??= new UserAccessService(
            _uow.UserDepartmentRoleRepository,
            _uow.RolePermissionRepository,
            _uow.UserTenantMembershipRepository);

    public async Task<int> SaveChangesAsync()
    {
        return await _uow.SaveChangesAsync();
    }
}