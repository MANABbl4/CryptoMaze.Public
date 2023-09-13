using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using System.Threading.Tasks;

namespace CryptoMaze.Client
{
    public interface ICryptoMazeLabirintGameClient
    {
        Task<WebCallResult<PlayerDataResponse>> GetPlayerDataAsync();
        Task<WebCallResult<SetNameResponse>> SetPlayerNameAsync(SetNameRequest request);
        Task<WebCallResult<StartGameResponse>> StartGameAsync();
        Task<WebCallResult<FinishLabirintResponse>> FinishLabirintAsync();
        Task<WebCallResult<CollectCryptoBlockResponse>> CollectCryptoBlockAsync(CollectCryptoBlockRequest request);
        Task<WebCallResult<CollectEnergyResponse>> CollectEnergyAsync(CollectEnergyRequest request);
        Task<WebCallResult<CollectCryptoKeyFragmentResponse>> CollectCryptoKeyFragmentAsync(CollectCryptoKeyFragmentRequest request);
        Task<WebCallResult<OpenStorageResponse>> OpenStorageAsync();
        Task<WebCallResult<UseSpeedRocketBoosterResponse>> UseSpeedRocketBoosterAsync();
        Task<WebCallResult<UseFreezeTimeBoosterResponse>> UseFreezeTimeBoosterAsync();
    }
}
