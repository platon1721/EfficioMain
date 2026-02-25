using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Security;
using DomainSec = Efficio.Domain.Security;
using DomainDep = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Mappers;

public class RoleMapper : IMapper<DalDto.Role, DomainSec.Role>
{
    public DalDto.Role? Map(DomainSec.Role? entity)
    {
        if (entity == null) return null;

        return new DalDto.Role
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            Department = entity.Department != null ? new DTO.Departments.Department
            {
                Id = entity.Department.Id,
                Name = entity.Department.Name,
                Description = entity.Department.Description
            } : null,
            RolePermissions = entity.RolePermissions?.Select(rp => new DalDto.RolePermission
            {
                Id = rp.Id,
                RoleId = rp.RoleId,
                PermissionId = rp.PermissionId,
                Permission = rp.Permission != null ? new DalDto.Permission
                {
                    Id = rp.Permission.Id,
                    Key = rp.Permission.Key,
                    Name = rp.Permission.Name,
                    ModuleId = rp.Permission.ModuleId
                } : null
            }).ToList()
        };
    }

    public DomainSec.Role? Map(DalDto.Role? entity)
    {
        if (entity == null) return null;

        return new DomainSec.Role
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}