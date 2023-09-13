using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface IGenericRepository<TEntity, TId>
        where TId : struct
        where TEntity : class, IEntity<TId>
    {
        Task<IEnumerable<TEntity>> GetAsync();
        ValueTask<TEntity?> GetByIdAsync(TId id);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);
        Task DeleteAsync(TEntity entity);
    }
}
