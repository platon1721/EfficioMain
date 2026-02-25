using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Mappers;

public class DepartmentInDepartmentMapper : IMapper<DalDto.DepartmentInDepartment, Dom.DepartmentInDepartment>
{
    public DalDto.DepartmentInDepartment? Map(Dom.DepartmentInDepartment? entity)
    {
        if (entity == null) return null;

        return new DalDto.DepartmentInDepartment
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ParentDepartmentId = entity.ParentDepartmentId,
            ChildDepartmentId = entity.ChildDepartmentId,
            ParentDepartment = entity.ParentDepartment != null ? new DalDto.Department
            {
                Id = entity.ParentDepartment.Id,
                Name = entity.ParentDepartment.Name,
                Description = entity.ParentDepartment.Description
            } : null,
            ChildDepartment = entity.ChildDepartment != null ? new DalDto.Department
            {
                Id = entity.ChildDepartment.Id,
                Name = entity.ChildDepartment.Name,
                Description = entity.ChildDepartment.Description
            } : null
        };
    }

    public Dom.DepartmentInDepartment? Map(DalDto.DepartmentInDepartment? entity)
    {
        if (entity == null) return null;

        return new Dom.DepartmentInDepartment
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ParentDepartmentId = entity.ParentDepartmentId,
            ChildDepartmentId = entity.ChildDepartmentId
        };
    }
}