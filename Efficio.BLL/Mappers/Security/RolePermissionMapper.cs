using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Security;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Mappers.Security;

public class RolePermissionMapper : IMapper<BllDto.RolePermission, DalDto.RolePermission>
{
    public BllDto.RolePermission? Map(DalDto.RolePermission? entity)
    {
        if (entity == null) return null;

        return new BllDto.RolePermission
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            RoleId = entity.RoleId,
            PermissionId = entity.PermissionId,
            Role = entity.Role != null ? new BllDto.Role
            {
                Id = entity.Role.Id,
                TenantRootDepartmentId = entity.Role.TenantRootDepartmentId,
                DepartmentId = entity.Role.DepartmentId,
                Name = entity.Role.Name,
                Description = entity.Role.Description
            } : null,
            Permission = entity.Permission != null ? new BllDto.Permission
            {
                Id = entity.Permission.Id,
                ModuleId = entity.Permission.ModuleId,
                Key = entity.Permission.Key,
                Name = entity.Permission.Name,
                IsActive = entity.Permission.IsActive
            } : null
        };
    }

    public DalDto.RolePermission? Map(BllDto.RolePermission? entity)
    {
        if (entity == null) return null;

        return new DalDto.RolePermission
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            RoleId = entity.RoleId,
            PermissionId = entity.PermissionId
        };
    }
}