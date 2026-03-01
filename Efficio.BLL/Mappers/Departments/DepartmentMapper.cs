using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Departments;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Mappers.Departments;

public class DepartmentMapper : IMapper<BllDto.Department, DalDto.Department>
{
    private readonly DepartmentTypeMapper _typeMapper = new();

    public BllDto.Department? Map(DalDto.Department? entity)
    {
        if (entity == null) return null;

        return new BllDto.Department
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentTypeId = entity.DepartmentTypeId,
            DepartmentType = _typeMapper.Map(entity.DepartmentType),
            ParentLinks = entity.ParentDepartmentLinks?.Select(l => new BllDto.DepartmentInDepartment
            {
                Id = l.Id,
                TenantRootDepartmentId = l.TenantRootDepartmentId,
                ParentDepartmentId = l.ParentDepartmentId,
                ChildDepartmentId = l.ChildDepartmentId
            }).ToList(),
            ChildLinks = entity.ChildDepartmentLinks?.Select(l => new BllDto.DepartmentInDepartment
            {
                Id = l.Id,
                TenantRootDepartmentId = l.TenantRootDepartmentId,
                ParentDepartmentId = l.ParentDepartmentId,
                ChildDepartmentId = l.ChildDepartmentId
            }).ToList()
        };
    }

    public DalDto.Department? Map(BllDto.Department? entity)
    {
        if (entity == null) return null;

        return new DalDto.Department
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentTypeId = entity.DepartmentTypeId
        };
    }
}