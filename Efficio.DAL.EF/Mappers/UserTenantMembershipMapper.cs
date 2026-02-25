using Base.Contracts;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Mappers;

public class UserTenantMembershipMapper : IMapper<DalDto.UserTenantMembership, Dom.UserTenantMembership>
{
    public DalDto.UserTenantMembership? Map(Dom.UserTenantMembership? entity)
    {
        if (entity == null) return null;

        return new DalDto.UserTenantMembership
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            UserId = entity.UserId,
            Status = (DalDto.UserMembershipStatus)entity.Status
        };
    }

    public Dom.UserTenantMembership? Map(DalDto.UserTenantMembership? entity)
    {
        if (entity == null) return null;

        return new Dom.UserTenantMembership
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            UserId = entity.UserId,
            Status = (Dom.UserMembershipStatus)entity.Status
        };
    }
}