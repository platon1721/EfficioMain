using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Repositories;

namespace Efficio.DAL.EF;

public class EfficioUOW : BaseUOW<EfficioDbContext>, IEfficioUOW
{
    private readonly IUserContext? _userContext;

    // Lazy fields
    private IModuleRepository? _moduleRepository;
    private IPermissionRepository? _permissionRepository;
    private ITenantRepository? _tenantRepository;
    private ITenantModuleRepository? _tenantModuleRepository;
    private IUserTenantMembershipRepository? _userTenantMembershipRepository;
    private IDepartmentRepository? _departmentRepository;
    private IDepartmentTypeRepository? _departmentTypeRepository;
    private IDepartmentInDepartmentRepository? _departmentInDepartmentRepository;
    private IRoleRepository? _roleRepository;
    private IRolePermissionRepository? _rolePermissionRepository;
    private IUserDepartmentRoleRepository? _userDepartmentRoleRepository;

    public EfficioUOW(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext)
    {
        _userContext = userContext;
    }

    public IModuleRepository ModuleRepository =>
        _moduleRepository ??= new ModuleRepository(UOWDbContext, _userContext);

    public IPermissionRepository PermissionRepository =>
        _permissionRepository ??= new PermissionRepository(UOWDbContext, _userContext);

    public ITenantRepository TenantRepository =>
        _tenantRepository ??= new TenantRepository(UOWDbContext, _userContext);

    public ITenantModuleRepository TenantModuleRepository =>
        _tenantModuleRepository ??= new TenantModuleRepository(UOWDbContext, _userContext);

    public IUserTenantMembershipRepository UserTenantMembershipRepository =>
        _userTenantMembershipRepository ??= new UserTenantMembershipRepository(UOWDbContext, _userContext);

    public IDepartmentRepository DepartmentRepository =>
        _departmentRepository ??= new DepartmentRepository(UOWDbContext, _userContext);

    public IDepartmentTypeRepository DepartmentTypeRepository =>
        _departmentTypeRepository ??= new DepartmentTypeRepository(UOWDbContext, _userContext);

    public IDepartmentInDepartmentRepository DepartmentInDepartmentRepository =>
        _departmentInDepartmentRepository ??= new DepartmentInDepartmentRepository(UOWDbContext, _userContext);

    public IRoleRepository RoleRepository =>
        _roleRepository ??= new RoleRepository(UOWDbContext, _userContext);

    public IRolePermissionRepository RolePermissionRepository =>
        _rolePermissionRepository ??= new RolePermissionRepository(UOWDbContext, _userContext);

    public IUserDepartmentRoleRepository UserDepartmentRoleRepository =>
        _userDepartmentRoleRepository ??= new UserDepartmentRoleRepository(UOWDbContext, _userContext);
}