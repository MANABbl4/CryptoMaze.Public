using CryptoMaze.ClientServer.Game.Responses;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public interface IShopService
    {
        Task<ShopDataResponse> GetDataAsync();
        Task<BuyResponse> BuyAsync(string email, int proposalId);
    }
}
