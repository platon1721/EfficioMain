using Base.Contracts;
using Base.DAL.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;


/// <summary>
/// Base repository with Guid key.
/// </summary>
public class BaseRepository<TDalEntity, TDomainEntity> : BaseRepository<TDalEntity, TDomainEntity, Guid>,
    IBaseRepository<TDalEntity>
    where TDalEntity : class, IDomainId
    where TDomainEntity : class, IDomainId
{
    public BaseRepository(DbContext repositoryDbContext, IMapper<TDalEntity, TDomainEntity> mapper,
        IUserContext? userContext = null)
        : base(repositoryDbContext, mapper, userContext)
    {
    }
}


/// <summary>
/// Filtering behavior:
/// - ITenantScoped entities: filtered by TenantRootDepartmentId
/// - IDepartmentScoped entities: filtered by DepartmentId (and TenantRootDepartmentId)
/// - Base repository with automatic scope filtering based on entity type and user context.
/// - Platform admins bypass all filtering
/// - Entities without scope interfaces: no automatic filtering
/// </summary>
public class BaseRepository<TDalEntity, TDomainEntity, TKey> : IBaseRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainId<TKey>
    where TDomainEntity : class, IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly DbContext RepositoryDbContext;
    protected readonly DbSet<TDomainEntity> RepositoryDbSet;
    protected readonly IMapper<TDalEntity, TDomainEntity, TKey> Mapper;
    protected readonly IUserContext? UserContext;

    public BaseRepository(DbContext repositoryDbContext, IMapper<TDalEntity, TDomainEntity, TKey> mapper,
        IUserContext? userContext = null)
    {
        RepositoryDbContext = repositoryDbContext;
        Mapper = mapper;
        UserContext = userContext;
        RepositoryDbSet = RepositoryDbContext.Set<TDomainEntity>();
    }


    
    /// <summary>
    /// Gets the base query with automatic scope filtering applied.
    /// Override this method to customize filtering behavior.
    /// </summary>
    protected virtual IQueryable<TDomainEntity> GetQuery()
    {
        var query = RepositoryDbSet.AsQueryable();

        // Platform admins bypass all filtering
        if (UserContext?.IsPlatformAdmin == true)
        {
            return query;
        }
        
        // Apply department scope filtering (includes tenant scope)
        if (IsDepartmentScoped() && UserContext?.DepartmentId.HasValue == true)
        {
            query = query.Where(e => ((IDepartmentScoped)e).DepartmentId == UserContext.DepartmentId.Value);
        }
        // Apply tenant scope filtering
        if (IsTenantScoped() && UserContext?.TenantRootDepartmentId.HasValue == true)
        {
            query = query.Where(e => ((ITenantScoped)e).TenantRootDepartmentId == UserContext.TenantRootDepartmentId.Value);
        }
        
        return query;
    }
    
    
    /// <summary>
    /// Checks if TDomainEntity implements ITenantScoped.
    /// </summary>
    protected bool IsTenantScoped()
    { 
        return typeof(ITenantScoped).IsAssignableFrom(typeof(TDomainEntity));
    }
    /// <summary>
    /// Checks if TDomainEntity implements IDepartmentScoped.
    /// </summary>
    protected bool IsDepartmentScoped()
    { 
        return typeof(IDepartmentScoped).IsAssignableFrom(typeof(TDomainEntity));
    }
    
    
    /// <summary>
    /// Applies scope values to entity before adding.
    /// </summary>
    protected virtual void ApplyScopeToEntity(TDomainEntity entity)
    {
        // Apply tenant scope
        if (entity is ITenantScoped tenantScoped && UserContext?.TenantRootDepartmentId.HasValue == true)
        {
            // Use reflection to set the value since the interface only has getter
            var property = entity.GetType().GetProperty(nameof(ITenantScoped.TenantRootDepartmentId));
            if (property?.CanWrite == true)
            {
                property.SetValue(entity, UserContext.TenantRootDepartmentId.Value);
            }
            else
            {
                // Try to find SetRootDepartmentId method (from BaseTenantEntity)
                var method = entity.GetType().GetMethod("SetRootDepartmentId");
                method?.Invoke(entity, new object[] { UserContext.TenantRootDepartmentId.Value });
            }
        }

        // Apply department scope
        if (entity is IDepartmentScoped departmentScoped && UserContext?.DepartmentId.HasValue == true)
        {
            var property = entity.GetType().GetProperty(nameof(IDepartmentScoped.DepartmentId));
            if (property?.CanWrite == true)
            {
                property.SetValue(entity, UserContext.DepartmentId.Value);
            }
            else
            {
                // Try to find SetDepartmentId method (from BaseDepartmentScopedEntity)
                var method = entity.GetType().GetMethod("SetDepartmentId");
                method?.Invoke(entity, new object[] { UserContext.DepartmentId.Value });
            }
        }
    }

    public virtual IEnumerable<TDalEntity> All()
    {
        return GetQuery()
            .ToList()
            .Select(e => Mapper.Map(e)!);
    }

    public virtual async Task<IEnumerable<TDalEntity>> AllAsync()
    {
        return (await GetQuery()
                .ToListAsync())
            .Select(e => Mapper.Map(e)!);
    }

    public virtual TDalEntity? Find(TKey id)
    {
        var query = GetQuery();
        var res = query.FirstOrDefault(e => e.Id.Equals(id));
        return Mapper.Map(res);
    }

    public virtual async Task<TDalEntity?> FindAsync(TKey id)
    {
        var query = GetQuery();
        var res = await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        return Mapper.Map(res);
    }

    public virtual void Add(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;
        ApplyScopeToEntity(domainEntity);
        RepositoryDbSet.Add(domainEntity);
    }
    
    public virtual async Task AddAsync(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;
        ApplyScopeToEntity(domainEntity);
        await RepositoryDbSet.AddAsync(domainEntity);
    }
    
    // ==================== Update Methods ====================

    public virtual TDalEntity? Update(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;
        
        // Verify entity belongs to current scope
        var existingEntity = GetQuery().AsNoTracking().FirstOrDefault(e => e.Id.Equals(entity.Id));
        if (existingEntity == null)
        {
            return null; // Entity not found or not in scope
        }

        // Preserve scope values from existing entity
        PreserveScopeValues(existingEntity, domainEntity);

        var updatedEntity = RepositoryDbSet.Update(domainEntity).Entity;
        return Mapper.Map(updatedEntity);

    }

    public virtual async Task<TDalEntity?> UpdateAsync(TDalEntity entity)
    {
        var domainEntity = Mapper.Map(entity)!;

        // Verify entity belongs to current scope
        var existingEntity = await GetQuery().AsNoTracking().FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));
        if (existingEntity == null)
        {
            return null; // Entity not found or not in scope
        }

        // Preserve scope values from existing entity
        PreserveScopeValues(existingEntity, domainEntity);

        var updatedEntity = RepositoryDbSet.Update(domainEntity).Entity;
        return Mapper.Map(updatedEntity);
    }
    
    
    /// <summary>
    /// Preserves scope values (tenant, department) from existing entity.
    /// Prevents scope manipulation through updates.
    /// </summary>
    protected virtual void PreserveScopeValues(TDomainEntity existingEntity, TDomainEntity updatedEntity)
    {
        if (existingEntity is ITenantScoped existingTenant && updatedEntity is ITenantScoped)
        {
            var property = updatedEntity.GetType().GetProperty(nameof(ITenantScoped.TenantRootDepartmentId));
            if (property?.CanWrite == true)
            {
                property.SetValue(updatedEntity, existingTenant.TenantRootDepartmentId);
            }
            else
            {
                var method = updatedEntity.GetType().GetMethod("SetRootDepartmentId");
                method?.Invoke(updatedEntity, new object[] { existingTenant.TenantRootDepartmentId });
            }
        }

        if (existingEntity is IDepartmentScoped existingDept && updatedEntity is IDepartmentScoped)
        {
            var property = updatedEntity.GetType().GetProperty(nameof(IDepartmentScoped.DepartmentId));
            if (property?.CanWrite == true)
            {
                property.SetValue(updatedEntity, existingDept.DepartmentId);
            }
            else
            {
                var method = updatedEntity.GetType().GetMethod("SetDepartmentId");
                method?.Invoke(updatedEntity, new object[] { existingDept.DepartmentId });
            }
        }
    }

    // ==================== Remove Methods ====================

    public virtual void Remove(TDalEntity entity)
    {
        Remove(entity.Id);
    }

    public virtual void Remove(TKey id)
    {
        var dbEntity = GetQuery().FirstOrDefault(e => e.Id.Equals(id));
        if (dbEntity != null)
        {
            RepositoryDbSet.Remove(dbEntity);
        }
    }

    public virtual async Task RemoveAsync(TKey id)
    {
        var dbEntity = await GetQuery().FirstOrDefaultAsync(e => e.Id.Equals(id));
        if (dbEntity != null)
        {
            RepositoryDbSet.Remove(dbEntity);
        }
    }

    // ==================== Exists Methods ====================

    public virtual bool Exists(TKey id)
    {
        return GetQuery().Any(e => e.Id.Equals(id));
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await GetQuery().AnyAsync(e => e.Id.Equals(id));
    }
}