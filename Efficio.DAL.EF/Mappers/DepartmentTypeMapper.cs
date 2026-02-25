using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Mappers;

public class DepartmentTypeMapper : IMapper<DalDto.DepartmentType, Dom.DepartmentType>
{
    public DalDto.DepartmentType? Map(Dom.DepartmentType? entity)
    {
        if (entity == null) return null;

        return new DalDto.DepartmentType
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public Dom.DepartmentType? Map(DalDto.DepartmentType? entity)
    {
        if (entity == null) return null;

        return new Dom.DepartmentType
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }
}