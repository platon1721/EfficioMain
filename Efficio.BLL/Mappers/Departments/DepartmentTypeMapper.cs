using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Departments;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Mappers.Departments;

public class DepartmentTypeMapper : IMapper<BllDto.DepartmentType, DalDto.DepartmentType>
{
    public BllDto.DepartmentType? Map(DalDto.DepartmentType? entity)
    {
        if (entity == null) return null;

        return new BllDto.DepartmentType
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public DalDto.DepartmentType? Map(BllDto.DepartmentType? entity)
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
}