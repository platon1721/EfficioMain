using Base.BLL.Contracts;
using Base.Contracts;
using Base.DAL.Contracts;

namespace Base.BLL;

public class BaseService<TBllEntity, TDalEntity, TRepository>
    : BaseService<TBllEntity, TDalEntity, TRepository, Guid>
    where TBllEntity : class, IDomainId
    where TDalEntity : class, IDomainId
    where TRepository : IBaseRepository<TDalEntity>
{
    public BaseService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
        : base(repository, mapper)
    {
    }
}

public class BaseService<TBllEntity, TDalEntity, TRepository, TKey>
    : IBaseService<TBllEntity, TKey>
    where TBllEntity : class, IDomainId<TKey>
    where TDalEntity : class, IDomainId<TKey>
    where TRepository : IBaseRepository<TDalEntity, TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly TRepository Repository;
    protected readonly IMapper<TBllEntity, TDalEntity, TKey> Mapper;

    public BaseService(TRepository repository, IMapper<TBllEntity, TDalEntity, TKey> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public virtual IEnumerable<TBllEntity> All()
    {
        return Repository.All().Select(e => Mapper.Map(e)!);
    }

    public virtual async Task<IEnumerable<TBllEntity>> AllAsync()
    {
        return (await Repository.AllAsync()).Select(e => Mapper.Map(e)!);
    }

    public virtual TBllEntity? Find(TKey id)
    {
        return Mapper.Map(Repository.Find(id));
    }

    public virtual async Task<TBllEntity?> FindAsync(TKey id)
    {
        return Mapper.Map(await Repository.FindAsync(id));
    }

    public virtual void Add(TBllEntity entity)
    {
        Repository.Add(Mapper.Map(entity)!);
    }

    public virtual TBllEntity? Update(TBllEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)!));
    }

    public virtual async Task<TBllEntity?> UpdateAsync(TBllEntity entity)
    {
        return Mapper.Map(await Repository.UpdateAsync(Mapper.Map(entity)!));
    }

    public virtual void Remove(TBllEntity entity)
    {
        Repository.Remove(Mapper.Map(entity)!);
    }

    public virtual void Remove(TKey id)
    {
        Repository.Remove(id);
    }

    public virtual async Task RemoveAsync(TKey id)
    {
        await Repository.RemoveAsync(id);
    }

    public virtual bool Exists(TKey id)
    {
        return Repository.Exists(id);
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await Repository.ExistsAsync(id);
    }
}