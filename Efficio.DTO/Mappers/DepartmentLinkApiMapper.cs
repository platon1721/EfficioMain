using BllDto = Efficio.BLL.DTO.Departments;
using Efficio.DTO.Departments.DepartmentHierarchy;

namespace Efficio.DTO.Mappers;

public static class DepartmentLinkApiMapper
{
    public static DepartmentLinkResponse ToResponse(BllDto.DepartmentInDepartment entity)
    {
        return new DepartmentLinkResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ParentDepartmentId = entity.ParentDepartmentId,
            ChildDepartmentId = entity.ChildDepartmentId,
            ParentDepartmentName = entity.ParentDepartment?.Name,
            ChildDepartmentName = entity.ChildDepartment?.Name
        };
    }
}