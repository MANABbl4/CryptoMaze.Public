using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class GenericRepository<TEntity, TId> : IGenericRepository<TEntity, TId>
        where TId : struct
        where TEntity : class, IEntity<TId>
    {
        protected readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(TEntity entity)
        {
            _dbContext.Add(entity);

            return _dbContext.SaveChangesAsync();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.AddRange(entities);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(TEntity entity)
        {
            _dbContext.Remove(entity);

            return _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public ValueTask<TEntity?> GetByIdAsync(TId id)
        {
            return _dbContext.Set<TEntity>().FindAsync(id);
        }

        public Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);

            return _dbContext.SaveChangesAsync();
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbContext.UpdateRange(entities);

            return _dbContext.SaveChangesAsync();
        }
    }
}
