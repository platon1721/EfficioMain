using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Mappers;

public class TenantModuleMapper : IMapper<DalDto.TenantModule, Dom.TenantModule>
{
    public DalDto.TenantModule? Map(Dom.TenantModule? entity)
    {
        if (entity == null) return null;

        return new DalDto.TenantModule
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ModuleId = entity.ModuleId,
            ExpiresAt = entity.ExpiresAt,
            Module = entity.Module != null ? new DTO.Security.Module
            {
                Id = entity.Module.Id,
                Code = entity.Module.Code,
                Name = entity.Module.Name,
                IsMain = entity.Module.IsMain,
                IsActive = entity.Module.IsActive
            } : null
        };
    }

    public Dom.TenantModule? Map(DalDto.TenantModule? entity)
    {
        if (entity == null) return null;

        return new Dom.TenantModule
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ModuleId = entity.ModuleId,
            ExpiresAt = entity.ExpiresAt
        };
    }
}