using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Security;
using DalDto = Efficio.DAL.DTO.Security;

namespace Efficio.BLL.Mappers.Security;

public class ModuleMapper : IMapper<BllDto.Module, DalDto.Module>
{
    public BllDto.Module? Map(DalDto.Module? entity)
    {
        if (entity == null) return null;

        return new BllDto.Module
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            Description = entity.Description,
            IsMain = entity.IsMain,
            IsActive = entity.IsActive
        };
    }

    public DalDto.Module? Map(BllDto.Module? entity)
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
}