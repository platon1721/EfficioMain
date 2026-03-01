using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Tenants;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Mappers.Tenants;

public class TenantMapper : IMapper<BllDto.Tenant, DalDto.Tenant>
{
    public BllDto.Tenant? Map(DalDto.Tenant? entity)
    {
        if (entity == null) return null;

        return new BllDto.Tenant
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            DefaultTimeZone = entity.DefaultTimeZone,
            DefaultLocale = entity.DefaultLocale,
            RootDepartmentId = entity.RootDepartmentId,
            Status = (BllDto.TenantStatus)entity.Status
        };
    }

    public DalDto.Tenant? Map(BllDto.Tenant? entity)
    {
        if (entity == null) return null;

        return new DalDto.Tenant
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            DefaultTimeZone = entity.DefaultTimeZone,
            DefaultLocale = entity.DefaultLocale,
            RootDepartmentId = entity.RootDepartmentId,
            Status = (DalDto.TenantStatus)entity.Status
        };
    }
}