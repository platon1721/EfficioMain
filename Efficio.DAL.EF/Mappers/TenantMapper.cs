using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Mappers;

public class TenantMapper : IMapper<DalDto.Tenant, Dom.Tenant>
{
    public DalDto.Tenant? Map(Dom.Tenant? entity)
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

    public Dom.Tenant? Map(DalDto.Tenant? entity)
    {
        if (entity == null) return null;

        return new Dom.Tenant
        {
            Id = entity.Id,
            Name = entity.Name,
            Code = entity.Code,
            DefaultTimeZone = entity.DefaultTimeZone,
            DefaultLocale = entity.DefaultLocale,
            RootDepartmentId = entity.RootDepartmentId,
            Status = (Dom.TenantStatus)entity.Status
        };
    }
}