using BllDto = Efficio.BLL.DTO.Tenants;
using Efficio.DTO.Tenants.UserTenantMembership;

namespace Efficio.DTO.Mappers;

public static class UserTenantMembershipApiMapper
{
    public static UserTenantMembershipResponse ToResponse(BllDto.UserTenantMembership entity)
    {
        return new UserTenantMembershipResponse
        {
            Id = entity.Id,
            TenantRootDepartmentId = entity.TenantRootDepartmentId,
            UserId = entity.UserId,
            Status = entity.Status.ToString()
        };
    }
}