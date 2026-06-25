using BllDto = Efficio.BLL.DTO.Tenants;
using Efficio.DTO.Tenants.Tenant;

namespace Efficio.DTO.Mappers;

public static class TenantApiMapper
{
    public static TenantResponse ToResponse(BllDto.Tenant entity)
    {
        return new TenantResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            DefaultTimeZone = entity.DefaultTimeZone,
            DefaultLocale = entity.DefaultLocale,
            RootDepartmentId = entity.RootDepartmentId,
            Status = entity.Status.ToString()
        };
    }

    public static BllDto.Tenant ToBll(CreateTenantRequest request)
    {
        return new BllDto.Tenant
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Code = request.Code.Trim().ToLowerInvariant(),
            DefaultTimeZone = request.DefaultTimeZone,
            DefaultLocale = request.DefaultLocale,
            Status = BllDto.TenantStatus.Active
        };
    }
}