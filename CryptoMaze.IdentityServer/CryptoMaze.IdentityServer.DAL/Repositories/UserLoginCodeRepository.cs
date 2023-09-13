using CryptoMaze.IdentityServer.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoMaze.IdentityServer.DAL.Repositories
{
    public class UserLoginCodeRepository : IUserLoginCodeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserLoginCodeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddAsync(UserLoginCode userLoginCode)
        {
            _dbContext.UserLoginCodes.Add(userLoginCode);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(UserLoginCode userLoginCode)
        {
            _dbContext.UserLoginCodes.Remove(userLoginCode);

            return _dbContext.SaveChangesAsync();
        }

        public Task<UserLoginCode> FindByIdAsync(int id)
        {
            return _dbContext.UserLoginCodes
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<UserLoginCode>> FindByUserEmailAsync(string email)
        {
            return await _dbContext.Users
                .Include(x => x.UserLoginCodes)
                .Where(x => x.Email == email)
                .SelectMany(x => x.UserLoginCodes)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserLoginCode>> FindByUserIdAsync(int userId)
        {
            return await _dbContext.Users
                .Include(x => x.UserLoginCodes)
                .Where(x => x.Id == userId)
                .SelectMany(x => x.UserLoginCodes)
                .ToListAsync();
        }

        public async Task<UserLoginCode> GetLastByUserEmailAsync(string email)
        {
            return await _dbContext.Users
                .Include(x => x.UserLoginCodes)
                .Where(x => x.Email == email)
                .SelectMany(x => x.UserLoginCodes)
                .OrderByDescending(x => x.Created)
                .FirstOrDefaultAsync();
        }

        public Task UpdateAsync(UserLoginCode userLoginCode)
        {
            _dbContext.Update(userLoginCode);

            return _dbContext.SaveChangesAsync();
        }
    }
}
