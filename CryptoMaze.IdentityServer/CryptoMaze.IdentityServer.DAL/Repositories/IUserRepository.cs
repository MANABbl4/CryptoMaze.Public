using CryptoMaze.IdentityServer.DAL.Entities;

namespace CryptoMaze.IdentityServer.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User> FindByIdAsync(int id);
        Task<User> FindByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
    }
}
