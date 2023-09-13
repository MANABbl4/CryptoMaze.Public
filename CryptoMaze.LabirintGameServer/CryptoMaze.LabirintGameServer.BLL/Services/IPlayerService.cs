using CryptoMaze.ClientServer.Game.Responses;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public interface IPlayerService
    {
        Task<PlayerDataResponse> GetDataAsync(string email);
        Task<SetNameResponse> SetNameAsync(string email, string name);
        Task<AddEnergyResponse> AddEnergyAsync(string email);
    }
}
