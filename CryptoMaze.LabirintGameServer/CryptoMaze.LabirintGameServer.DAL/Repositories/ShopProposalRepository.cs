using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class ShopProposalRepository : GenericRepository<ShopProposal, int>, IShopProposalRepository
    {
        public ShopProposalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public new async Task<IEnumerable<ShopProposal>> GetAsync()
        {
            return await _dbContext.ShopProposals
                .Include(x => x.BuyItem)
                .Include(x => x.SellItem)
                .ToListAsync();
        }
    }
}
