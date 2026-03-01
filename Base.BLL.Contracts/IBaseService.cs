using Base.Contracts;

namespace Base.BLL.Contracts;

public interface IBaseService<TBllEntity> : IBaseService<TBllEntity, Guid>
    where TBllEntity : class, IDomainId
{
}

public interface IBaseService<TBllEntity, TKey>
    where TBllEntity : class, IDomainId<TKey>
    where TKey : IEquatable<TKey>
{
    IEnumerable<TBllEntity> All();
    Task<IEnumerable<TBllEntity>> AllAsync();

    TBllEntity? Find(TKey id);
    Task<TBllEntity?> FindAsync(TKey id);

    void Add(TBllEntity entity);

    TBllEntity? Update(TBllEntity entity);
    Task<TBllEntity?> UpdateAsync(TBllEntity entity);

    void Remove(TBllEntity entity);
    void Remove(TKey id);
    Task RemoveAsync(TKey id);

    bool Exists(TKey id);
    Task<bool> ExistsAsync(TKey id);
}