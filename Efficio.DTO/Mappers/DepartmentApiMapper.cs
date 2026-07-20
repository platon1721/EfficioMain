using BllDto = Efficio.BLL.DTO.Departments;
using Efficio.DTO.Departments.Department;

namespace Efficio.DTO.Mappers;

public static class DepartmentApiMapper
{
    public static DepartmentResponse ToResponse(BllDto.Department entity)
    {
        return new DepartmentResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            Name = entity.Name,
            Description = entity.Description,
            DepartmentTypeId = entity.DepartmentTypeId,
            DepartmentTypeName = entity.DepartmentType?.Name
        };
    }

    public static BllDto.Department ToBll(CreateDepartmentRequest request, Guid tenantRootDepartmentId)
    {
        return new BllDto.Department
        {
            Id = Guid.NewGuid(),
            TenantRootDepartmentId = tenantRootDepartmentId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            DepartmentTypeId = request.DepartmentTypeId
        };
    }
}