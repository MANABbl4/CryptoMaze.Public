using CryptoMaze.Common;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class UserItemRepository : GenericRepository<UserItem, int>, IUserItemRepository
    {
        public UserItemRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<UserItem>> GetUserItems(string email)
        {
            return await _dbContext.UserItems
                .Where(x => x.User.Email == email)
                .Include(x => x.Item)
                .Include(x => x.User)
                .ToListAsync();
        }

        public Task<UserItem> GetUserItem(string email, ItemType type)
        {
            return _dbContext.UserItems
                .Where(x => x.User.Email == email)
                .Include(x => x.Item)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Item.Type == type);
        }
    }
}
