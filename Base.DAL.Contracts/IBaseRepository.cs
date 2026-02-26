using Base.Contracts;

namespace Base.DAL.Contracts;

public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
    where TEntity : IDomainId<Guid>
{
    
}

public interface IBaseRepository<TEntity, TKey>
    where TEntity : IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    IEnumerable<TEntity> All();
    Task<IEnumerable<TEntity>> AllAsync();

    TEntity? Find(TKey id);
    Task<TEntity?> FindAsync(TKey id);

    void Add(TEntity entity);

    TEntity? Update(TEntity entity);
    Task<TEntity?> UpdateAsync(TEntity entity);

    void Remove(TEntity entity);

    void Remove(TKey id);
    Task RemoveAsync(TKey id);

    bool Exists(TKey id);
    Task<bool> ExistsAsync(TKey id);
}