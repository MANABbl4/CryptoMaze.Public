using CryptoMaze.IdentityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.IdentityServer.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);

            return _dbContext.SaveChangesAsync();
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> FindByIdAsync(int id)
        {
            return _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task UpdateAsync(User user)
        {
            _dbContext.Update(user);

            return _dbContext.SaveChangesAsync();
        }
    }
}
