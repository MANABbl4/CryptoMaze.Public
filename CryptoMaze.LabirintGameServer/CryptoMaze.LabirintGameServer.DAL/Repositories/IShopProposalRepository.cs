using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface IShopProposalRepository : IGenericRepository<ShopProposal, int>
    {
        Task<IEnumerable<ShopProposal>> GetAsync();
    }
}
