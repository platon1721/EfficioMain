using BllDto = Efficio.BLL.DTO.Tenants;
using Efficio.DTO.Tenants.TenantModule;

namespace Efficio.DTO.Mappers;

public static class TenantModuleApiMapper
{
    public static TenantModuleResponse ToResponse(BllDto.TenantModule entity)
    {
        return new TenantModuleResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            ModuleId = entity.ModuleId,
            ModuleName = entity.Module?.Name,
            ModuleCode = entity.Module?.Code,
            ExpiresAt = entity.ExpiresAt
        };
    }
}