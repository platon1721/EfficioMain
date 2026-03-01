using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Departments;
using DalDto = Efficio.DAL.DTO.Departments;

namespace Efficio.BLL.Mappers.Departments;

public class DepartmentInDepartmentMapper : IMapper<BllDto.DepartmentInDepartment, DalDto.DepartmentInDepartment>
{
    private readonly DepartmentMapper _departmentMapper = new();

    public BllDto.DepartmentInDepartment? Map(DalDto.DepartmentInDepartment? entity)
    {
        if (entity == null) return null;

        return new BllDto.DepartmentInDepartment
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ParentDepartmentId = entity.ParentDepartmentId,
            ChildDepartmentId = entity.ChildDepartmentId,
            ParentDepartment = _departmentMapper.Map(entity.ParentDepartment),
            ChildDepartment = _departmentMapper.Map(entity.ChildDepartment)
        };
    }

    public DalDto.DepartmentInDepartment? Map(BllDto.DepartmentInDepartment? entity)
    {
        if (entity == null) return null;

        return new DalDto.DepartmentInDepartment
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ParentDepartmentId = entity.ParentDepartmentId,
            ChildDepartmentId = entity.ChildDepartmentId
        };
    }
}