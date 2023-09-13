using CryptoMaze.LabirintGameServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public class UserRepository : GenericRepository<User, Guid>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _dbContext.Users
                .Include(x => x.Items)
                .ThenInclude(x => x.Item)
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
