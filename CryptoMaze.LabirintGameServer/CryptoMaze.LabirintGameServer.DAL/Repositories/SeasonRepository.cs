using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class SeasonRepository : GenericRepository<Season, int>, ISeasonRepository
    {
        public SeasonRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Season> GetSeasonAsync(DateTime date)
        {
            return _dbContext.Seasons
                .Where(x => x.StartDate < date && x.FinishDate > date)
                .FirstOrDefaultAsync();
        }

        public Task<Season> GetPreviousSeasonAsync(DateTime date)
        {
            return _dbContext.Seasons
                .Where(x => x.FinishDate < date)
                .OrderByDescending(x => x.FinishDate)
                .FirstOrDefaultAsync();
        }
    }
}
