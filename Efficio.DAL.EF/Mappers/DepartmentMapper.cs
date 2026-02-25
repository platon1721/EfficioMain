using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Mappers;

public class DepartmentMapper : IMapper<DalDto.Department, Dom.Department>
{
    public DalDto.Department? Map(Dom.Department? entity)
    {
        if (entity == null) return null;

        return new DalDto.Department
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentTypeId = entity.DepartmentTypeId,
            DepartmentType = entity.DepartmentType != null ? new DalDto.DepartmentType
            {
                Id = entity.DepartmentType.Id,
                TenantRootDepartmentId = entity.DepartmentType.TenantRootDepartmentId,
                Name = entity.DepartmentType.Name,
                Description = entity.DepartmentType.Description
            } : null,
            ParentDepartmentLinks = entity.ParentDepartmentLinks?.Select(link => new DalDto.DepartmentInDepartment
            {
                Id = link.Id,
                TenantRootDepartmentId = link.TenantRootDepartmentId,
                ParentDepartmentId = link.ParentDepartmentId,
                ChildDepartmentId = link.ChildDepartmentId
            }).ToList(),
            ChildDepartmentLinks = entity.ChildDepartmentLinks?.Select(link => new DalDto.DepartmentInDepartment
            {
                Id = link.Id,
                TenantRootDepartmentId = link.TenantRootDepartmentId,
                ParentDepartmentId = link.ParentDepartmentId,
                ChildDepartmentId = link.ChildDepartmentId
            }).ToList()
        };
    }

    public Dom.Department? Map(DalDto.Department? entity)
    {
        if (entity == null) return null;

        return new Dom.Department
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentTypeId = entity.DepartmentTypeId
        };
    }
}