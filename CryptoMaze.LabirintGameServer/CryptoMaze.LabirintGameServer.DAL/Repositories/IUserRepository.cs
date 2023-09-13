using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
