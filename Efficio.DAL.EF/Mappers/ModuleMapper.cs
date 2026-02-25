using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Security;
using Dom = Efficio.Domain.Security;

namespace Efficio.DAL.EF.Mappers;

public class ModuleMapper : IMapper<DalDto.Module, Dom.Module>
{
    public DalDto.Module? Map(Dom.Module? entity)
    {
        if (entity == null) return null;

        return new DalDto.Module
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsMain = entity.IsMain,
            IsActive = entity.IsActive
        };
    }

    public Dom.Module? Map(DalDto.Module? entity)
    {
        if (entity == null) return null;

        return new Dom.Module
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsMain = entity.IsMain,
            IsActive = entity.IsActive
        };
    }
}