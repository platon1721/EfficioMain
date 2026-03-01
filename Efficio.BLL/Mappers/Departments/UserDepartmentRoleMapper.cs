using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Departments;
using DalDto = Efficio.DAL.DTO.Departments;
using BllSec = Efficio.BLL.DTO.Security;
using DalSec = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Mappers.Departments;

public class UserDepartmentRoleMapper : IMapper<BllDto.UserDepartmentRole, DalDto.UserDepartmentRole>
{
    public BllDto.UserDepartmentRole? Map(DalDto.UserDepartmentRole? entity)
    {
        if (entity == null) return null;

        return new BllDto.UserDepartmentRole
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            UserId = entity.UserId,
            RoleId = entity.RoleId,
            Role = entity.Role != null ? new BllSec.Role
            {
                Id = entity.Role.Id,
                TenantRootDepartmentId = entity.Role.TenantRootDepartmentId,
                DepartmentId = entity.Role.DepartmentId,
                Name = entity.Role.Name,
                Description = entity.Role.Description
            } : null,
            Department = entity.Department != null ? new BllDto.Department
            {
                Id = entity.Department.Id,
                TenantRootDepartmentId = entity.Department.TenantRootDepartmentId,
                Name = entity.Department.Name,
                Description = entity.Department.Description,
                DepartmentTypeId = entity.Department.DepartmentTypeId
            } : null
        };
    }

    public DalDto.UserDepartmentRole? Map(BllDto.UserDepartmentRole? entity)
    {
        if (entity == null) return null;

        return new DalDto.UserDepartmentRole
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            UserId = entity.UserId,
            RoleId = entity.RoleId
        };
    }
}