using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.BLL.Models;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public interface ILeaderboardService
    {
        Task<LeaderboardData> GetCurrentSeasonDataAsync();
        Task<LeaderboardDataResponse> GetDataAsync(string email, LeaderboardData leaderboardData);
        Task<FinishSeasonResponse> FinishSeasonAsync();
    }
}
