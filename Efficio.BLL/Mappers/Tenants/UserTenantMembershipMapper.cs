using Base.Contracts;
using BllDto = Efficio.BLL.DTO.Tenants;
using DalDto = Efficio.DAL.DTO.Tenants;

namespace Efficio.BLL.Mappers.Tenants;

public class UserTenantMembershipMapper : IMapper<BllDto.UserTenantMembership, DalDto.UserTenantMembership>
{
    public BllDto.UserTenantMembership? Map(DalDto.UserTenantMembership? entity)
    {
        if (entity == null) return null;

        return new BllDto.UserTenantMembership
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            UserId = entity.UserId,
            Status = (BllDto.UserMembershipStatus)entity.Status
        };
    }

    public DalDto.UserTenantMembership? Map(BllDto.UserTenantMembership? entity)
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
}