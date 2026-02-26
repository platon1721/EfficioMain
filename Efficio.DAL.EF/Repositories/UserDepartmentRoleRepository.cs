using Base.Contracts;
using Base.DAL.EF;
using Efficio.DAL.Contracts;
using Efficio.DAL.EF.Mappers;
using Microsoft.EntityFrameworkCore;
using DalDto = Efficio.DAL.DTO.Departments;
using Dom = Efficio.Domain.Departments;

namespace Efficio.DAL.EF.Repositories;

public class UserDepartmentRoleRepository : BaseRepository<DalDto.UserDepartmentRole, Dom.UserDepartmentRole>, IUserDepartmentRoleRepository
{
    public UserDepartmentRoleRepository(EfficioDbContext dbContext, IUserContext? userContext = null)
        : base(dbContext, new UserDepartmentRoleMapper(), userContext)
    {
    }

    public async Task<IEnumerable<DalDto.UserDepartmentRole>> GetByUserAsync(Guid userId)
    {
        var entities = await RepositoryDbSet
            .Include(udr => udr.Role)
            .Include(udr => udr.Department)
            .Where(udr => udr.UserId == userId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<IEnumerable<DalDto.UserDepartmentRole>> GetByDepartmentAsync(Guid departmentId)
    {
        var entities = await RepositoryDbSet
            .Include(udr => udr.Role)
            .Include(udr => udr.User)
            .Where(udr => udr.DepartmentId == departmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public Task<IEnumerable<DalDto.UserDepartmentRole>> GetByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DalDto.UserDepartmentRole>> GetByRoleIdAsync(Guid roleId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DalDto.UserDepartmentRole>> GetByDepartmentIdAsync(Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<DalDto.UserDepartmentRole?> FindByUserAndDepartmentAsync(Guid userId, Guid departmentId)
    {
        var entity = await RepositoryDbSet
            .Include(udr => udr.Role)
            .Include(udr => udr.Department)
            .FirstOrDefaultAsync(udr => udr.UserId == userId && udr.DepartmentId == departmentId);
        return Mapper.Map(entity);
    }

    public Task<bool> UserHasRoleInDepartmentAsync(Guid userId, Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DalDto.UserDepartmentRole>> GetWithRoleAndDepartmentAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<string>> GetUserPermissionKeysAsync(Guid userId, Guid departmentId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<DalDto.UserDepartmentRole>> GetByRoleAsync(Guid roleId)
    {
        var entities = await RepositoryDbSet
            .Include(udr => udr.User)
            .Include(udr => udr.Department)
            .Where(udr => udr.RoleId == roleId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> HasRoleInDepartmentAsync(Guid userId, Guid departmentId)
    {
        return await RepositoryDbSet
            .AnyAsync(udr => udr.UserId == userId && udr.DepartmentId == departmentId);
    }

    public async Task<IEnumerable<DalDto.UserDepartmentRole>> GetByUserInTenantAsync(Guid userId, Guid tenantRootDepartmentId)
    {
        var entities = await RepositoryDbSet
            .Include(udr => udr.Role)
            .Include(udr => udr.Department)
            .Where(udr => udr.UserId == userId && udr.TenantRootDepartmentId == tenantRootDepartmentId)
            .ToListAsync();
        return entities.Select(e => Mapper.Map(e)!);
    }
}