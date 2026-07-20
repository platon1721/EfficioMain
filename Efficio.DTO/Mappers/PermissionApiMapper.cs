using BllDto = Efficio.BLL.DTO.Security;
using Efficio.DTO.Security.Permission;

namespace Efficio.DTO.Mappers;

public static class PermissionApiMapper
{
    public static PermissionResponse ToResponse(BllDto.Permission entity)
    {
        return new PermissionResponse
        {
            Id = entity.Id,
            ModuleId = entity.ModuleId,
            ModuleName = entity.Module?.Name,
            Key = entity.Key,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }

    public static BllDto.Permission ToBll(CreatePermissionRequest request)
    {
        return new BllDto.Permission
        {
            Id = Guid.NewGuid(),
            ModuleId = request.ModuleId,
            Key = request.Key.Trim(),
            Name = request.Name?.Trim(),
            IsActive = request.IsActive
        };
    }
}