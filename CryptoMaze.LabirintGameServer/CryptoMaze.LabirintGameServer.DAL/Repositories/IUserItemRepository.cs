using CryptoMaze.Common;
using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface IUserItemRepository : IGenericRepository<UserItem, int>
    {
        Task<IEnumerable<UserItem>> GetUserItems(string email);
        Task<UserItem> GetUserItem(string email, ItemType type);
    }
}
