using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Tenants;
using Dom = Efficio.Domain.Tenants;

namespace Efficio.DAL.EF.Repositories;

public class UserTenantMembershipRepository : BaseRepository<DalDto.UserTenantMembership, Dom.UserTenantMembership>, IUserTenantMembershipRepository
{
    public UserTenantMembershipRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new UserTenantMembershipMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.UserTenantMembership>> GetByUserIdAsync(Guid userId)
    {
        var entities = await GetQuery()
            .Where(m => m.UserId == userId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.UserTenantMembership>> GetByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(m => m.User)
            .Where(m => m.TenantRootDepartmentId == tenantRootDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<DalDto.UserTenantMembership?> FindByUserAndTenantAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        var entity = await GetQuery()
            .FirstOrDefaultAsync(m => m.UserId == userId &&
                                      m.TenantRootDepartmentId == tenantRootDepartmentId);
        return Mapper.Map(entity);
    }

    public async Task<IEnumerable<DalDto.UserTenantMembership>> GetActiveUsersByTenantAsync(Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(m => m.User)
            .Where(m => m.TenantRootDepartmentId == tenantRootDepartmentId &&
                        m.Status == Dom.UserMembershipStatus.Active)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> IsMemberAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        return await GetQuery()
            .AnyAsync(m => m.UserId == userId &&
                           m.TenantRootDepartmentId == tenantRootDepartmentId &&
                           m.Status == Dom.UserMembershipStatus.Active);
    }
}