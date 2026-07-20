using BllDto = Efficio.BLL.DTO.Security;
using Efficio.DTO.Security.Module;

namespace Efficio.DTO.Mappers;

public static class ModuleApiMapper
{
    public static ModuleResponse ToResponse(BllDto.Module entity)
    {
        return new ModuleResponse
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsMain = entity.IsMain,
            IsActive = entity.IsActive
        };
    }

    public static BllDto.Module ToBll(CreateModuleRequest request)
    {
        return new BllDto.Module
        {
            Id = Guid.NewGuid(),
            Code = request.Code.Trim(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            IsMain = request.IsMain,
            IsActive = request.IsActive
        };
    }
}