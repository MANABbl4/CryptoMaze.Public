using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class GameRepository : GenericRepository<Game, int>, IGameRepository
    {
        public GameRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Game> GetLastGame(string email)
        {
            return _dbContext.Games.Where(x => x.User.Email == email)
                .OrderByDescending(x => x.Id)
                .Include(x => x.Labirints)
                .ThenInclude(x => x.CryptoBlocks)
                .Include(x => x.Labirints)
                .ThenInclude(x => x.Energies)
                .Include(x => x.Labirints)
                .ThenInclude(x => x.CryptoKeyFragments)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Game>> GetGameResults(DateTime startTime, DateTime finishTime)
        {
            return await _dbContext.Games
                .Where(x => x.FinishTime.HasValue && x.FinishTime > startTime && x.FinishTime < finishTime)
                .Include(x => x.User)
                .ToListAsync();
        }
    }
}
