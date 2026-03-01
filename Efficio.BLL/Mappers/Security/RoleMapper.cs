using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Security;
using BllDep = Efficio.BLL.DTO.Departments;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Mappers.Security;

public class RoleMapper : IMapper<BllDto.Role, DalDto.Role>
{
    public BllDto.Role? Map(DalDto.Role? entity)
    {
        if (entity == null) return null;

        return new BllDto.Role
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            Department = entity.Department != null ? new BllDep.Department
            {
                Id = entity.Department.Id,
                TenantRootDepartmentId = entity.Department.TenantRootDepartmentId,
                Name = entity.Department.Name,
                Description = entity.Department.Description,
                DepartmentTypeId = entity.Department.DepartmentTypeId
            } : null,
            RolePermissions = entity.RolePermissions?.Select(rp => new BllDto.RolePermission
            {
                Id = rp.Id,
                TenantRootDepartmentId = rp.TenantRootDepartmentId,
                DepartmentId = rp.DepartmentId,
                RoleId = rp.RoleId,
                PermissionId = rp.PermissionId,
                Permission = rp.Permission != null ? new BllDto.Permission
                {
                    Id = rp.Permission.Id,
                    ModuleId = rp.Permission.ModuleId,
                    Key = rp.Permission.Key,
                    Name = rp.Permission.Name,
                    IsActive = rp.Permission.IsActive
                } : null
            }).ToList()
        };
    }

    public DalDto.Role? Map(BllDto.Role? entity)
    {
        if (entity == null) return null;

        return new DalDto.Role
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}