using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface IGameRepository : IGenericRepository<Game, int>
    {
        Task<Game> GetLastGame(string email);
        Task<IEnumerable<Game>> GetGameResults(DateTime startTime, DateTime finishTime);
    }
}
