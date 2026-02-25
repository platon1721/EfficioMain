using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Mappers;

public class RolePermissionMapper : IMapper<DalDto.RolePermission, Dom.RolePermission>
{
    public DalDto.RolePermission? Map(Dom.RolePermission? entity)
    {
        if (entity == null) return null;

        return new DalDto.RolePermission
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            RoleId = entity.RoleId,
            PermissionId = entity.PermissionId,
            Role = entity.Role != null ? new DalDto.Role
            {
                Id = entity.Role.Id,
                Name = entity.Role.Name,
                Description = entity.Role.Description
            } : null,
            Permission = entity.Permission != null ? new DalDto.Permission
            {
                Id = entity.Permission.Id,
                Key = entity.Permission.Key,
                Name = entity.Permission.Name,
                ModuleId = entity.Permission.ModuleId
            } : null
        };
    }

    public Dom.RolePermission? Map(DalDto.RolePermission? entity)
    {
        if (entity == null) return null;

        return new Dom.RolePermission
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            RoleId = entity.RoleId,
            PermissionId = entity.PermissionId
        };
    }
}