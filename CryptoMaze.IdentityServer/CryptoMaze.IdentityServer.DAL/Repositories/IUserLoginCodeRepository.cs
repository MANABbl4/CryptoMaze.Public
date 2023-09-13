using CryptoMaze.IdentityServer.DAL.Entities;

namespace CryptoMaze.IdentityServer.DAL.Repositories
{
    public interface IUserLoginCodeRepository
    {
        Task<UserLoginCode> FindByIdAsync(int id);
        Task<IEnumerable<UserLoginCode>> FindByUserIdAsync(int userId);
        Task<IEnumerable<UserLoginCode>> FindByUserEmailAsync(string email);
        Task<UserLoginCode> GetLastByUserEmailAsync(string email);
        Task AddAsync(UserLoginCode userLoginCode);
        Task UpdateAsync(UserLoginCode userLoginCode);
        Task DeleteAsync(UserLoginCode userLoginCode);
    }
}
