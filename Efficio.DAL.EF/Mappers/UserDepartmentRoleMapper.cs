using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Mappers;

public class UserDepartmentRoleMapper : IMapper<DalDto.UserDepartmentRole, Dom.UserDepartmentRole>
{
    public DalDto.UserDepartmentRole? Map(Dom.UserDepartmentRole? entity)
    {
        if (entity == null) return null;

        return new DalDto.UserDepartmentRole
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            UserId = entity.UserId,
            RoleId = entity.RoleId,
            Role = entity.Role != null ? new DTO.Security.Role
            {
                Id = entity.Role.Id,
                Name = entity.Role.Name,
                Description = entity.Role.Description
            } : null,
            Department = entity.Department != null ? new DalDto.Department
            {
                Id = entity.Department.Id,
                Name = entity.Department.Name,
                Description = entity.Department.Description
            } : null
        };
    }

    public Dom.UserDepartmentRole? Map(DalDto.UserDepartmentRole? entity)
    {
        if (entity == null) return null;

        return new Dom.UserDepartmentRole
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            DepartmentId = entity.DepartmentId,
            UserId = entity.UserId,
            RoleId = entity.RoleId
        };
    }
}