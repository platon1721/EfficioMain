using BllDto = Efficio.BLL.DTO.Departments;
using Efficio.DTO.Departments.DepartmentType;

namespace Efficio.DTO.Mappers;

public static class DepartmentTypeApiMapper
{
    public static DepartmentTypeResponse ToResponse(BllDto.DepartmentType entity)
    {
        return new DepartmentTypeResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static BllDto.DepartmentType ToBll(CreateDepartmentTypeRequest request, Guid tenantRootDepartmentId)
    {
        return new BllDto.DepartmentType
        {
            Id = Guid.NewGuid(),
            TenantRootDepartmentId = tenantRootDepartmentId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };
    }
}