using BllDto = Efficio.BLL.DTO.Security;
using Efficio.DTO.Security.Permission;
using Efficio.DTO.Security.Role;

namespace Efficio.DTO.Mappers;

public static class RoleApiMapper
{
    public static RoleResponse ToResponse(BllDto.Role entity)
    {
        return new RoleResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentName = entity.Department?.Name
        };
    }

    public static RoleWithPermissionsResponse ToDetailResponse(BllDto.Role entity)
    {
        return new RoleWithPermissionsResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentName = entity.Department?.Name,
            Permissions = entity.RolePermissions?
                .Where(rp => rp.Permission != null)
                .Select(rp => new PermissionResponse
                {
                    Id = rp.Permission!.Id,
                    ModuleId = rp.Permission.ModuleId,
                    Key = rp.Permission.Key,
                    Name = rp.Permission.Name,
                    IsActive = rp.Permission.IsActive
                }).ToList() ?? new List<PermissionResponse>()
        };
    }

    public static BllDto.Role ToBll(CreateRoleRequest request, Guid tenantRootDepartmentId)
    {
        return new BllDto.Role
        {
            Id = Guid.NewGuid(),
            TenantRootDepartmentId = tenantRootDepartmentId,
            DepartmentId = request.DepartmentId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };
    }
}