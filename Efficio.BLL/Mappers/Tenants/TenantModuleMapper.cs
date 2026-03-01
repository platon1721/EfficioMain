using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Tenants;
using BllSec = Efficio.BLL.DTO.Security;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Mappers.Tenants;

public class TenantModuleMapper : IMapper<BllDto.TenantModule, DalDto.TenantModule>
{
    public BllDto.TenantModule? Map(DalDto.TenantModule? entity)
    {
        if (entity == null) return null;

        return new BllDto.TenantModule
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ModuleId = entity.ModuleId,
            ExpiresAt = entity.ExpiresAt,
            Module = entity.Module != null ? new BllSec.Module
            {
                Id = entity.Module.Id,
                Code = entity.Module.Code,
                Name = entity.Module.Name,
                Description = entity.Module.Description,
                IsMain = entity.Module.IsMain,
                IsActive = entity.Module.IsActive
            } : null
        };
    }

    public DalDto.TenantModule? Map(BllDto.TenantModule? entity)
    {
        if (entity == null) return null;

        return new DalDto.TenantModule
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ModuleId = entity.ModuleId,
            ExpiresAt = entity.ExpiresAt
        };
    }
}