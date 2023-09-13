using CryptoMaze.LabirintGameServer.DAL.Entities;

namespace CryptoMaze.LabirintGameServer.DAL.Repositories
{
    public interface ISeasonRepository : IGenericRepository<Season, int>
    {
        Task<Season> GetSeasonAsync(DateTime date);
        Task<Season> GetPreviousSeasonAsync(DateTime date);
    }
}
