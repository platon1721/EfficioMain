using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Security;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Mappers.Security;

public class PermissionMapper : IMapper<BllDto.Permission, DalDto.Permission>
{
    private readonly ModuleMapper _moduleMapper = new();

    public BllDto.Permission? Map(DalDto.Permission? entity)
    {
        if (entity == null) return null;

        return new BllDto.Permission
        {
            Id = entity.Id,
            ModuleId = entity.ModuleId,
            Key = entity.Key,
            Name = entity.Name,
            IsActive = entity.IsActive,
            Module = _moduleMapper.Map(entity.Module)
        };
    }

    public DalDto.Permission? Map(BllDto.Permission? entity)
    {
        if (entity == null) return null;

        return new DalDto.Permission
        {
            Id = entity.Id,
            ModuleId = entity.ModuleId,
            Key = entity.Key,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }
}