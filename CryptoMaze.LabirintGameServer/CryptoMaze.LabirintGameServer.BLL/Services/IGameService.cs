using CryptoMaze.ClientServer.Game.Responses;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public interface IGameService
    {
        Task<StartGameResponse> StartGameAsync(string email);
        Task<FinishGameResponse> FinishGameAsync(string email);
        Task<StartLabirintResponse> StartLabirintAsync(string email);
        Task<FinishLabirintResponse> FinishLabirintAsync(string email);
        Task<CollectCryptoBlockResponse> CollectCryptoBlockAsync(string email, Guid id);
        Task<CollectEnergyResponse> CollectEnergyAsync(string email, Guid id);
        Task<CollectCryptoKeyFragmentResponse> CollectCryptoKeyFragmentAsync(string email, Guid id);
        Task<OpenStorageResponse> OpenStorageAsync(string email);
        Task<UseSpeedRocketBoosterResponse> UseSpeedRocketBoosterAsync(string email);
        Task<UseFreezeTimeBoosterResponse> UseFreezeTimeBoosterAsync(string email);
    }
}
