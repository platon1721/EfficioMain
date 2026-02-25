using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Mappers;

public class PermissionMapper : IMapper<DalDto.Permission, Dom.Permission>
{
    public DalDto.Permission? Map(Dom.Permission? entity)
    {
        if (entity == null) return null;

        return new DalDto.Permission
        {
            Id = entity.Id,
            ModuleId = entity.ModuleId,
            Key = entity.Key,
            Name = entity.Name,
            IsActive = entity.IsActive,
            Module = entity.Module != null ? new DalDto.Module
            {
                Id = entity.Module.Id,
                Code = entity.Module.Code,
                Name = entity.Module.Name,
                IsMain = entity.Module.IsMain,
                IsActive = entity.Module.IsActive
            } : null
        };
    }

    public Dom.Permission? Map(DalDto.Permission? entity)
    {
        if (entity == null) return null;

        return new Dom.Permission
        {
            Id = entity.Id,
            ModuleId = entity.ModuleId,
            Key = entity.Key,
            Name = entity.Name,
            IsActive = entity.IsActive
        };
    }
}