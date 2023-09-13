using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using RSG;

namespace CryptoMaze.Client
{
    public interface ICryptoMazeLabirintGameClient
    {
        CryptoMazeClientOptions Options { get; }
        IPromise<PlayerDataResponse> GetPlayerData();
        IPromise<SetNameResponse> SetPlayerName(SetNameRequest request);
        IPromise<AddEnergyResponse> AddEnergy();

        IPromise<StartGameResponse> StartGame();
        IPromise<StartLabirintResponse> StartLabirint();
        IPromise<FinishLabirintResponse> FinishLabirint();
        IPromise<FinishGameResponse> FinishGame();
        IPromise<CollectCryptoBlockResponse> CollectCryptoBlock(CollectCryptoBlockRequest request);
        IPromise<CollectEnergyResponse> CollectEnergy(CollectEnergyRequest request);
        IPromise<CollectCryptoKeyFragmentResponse> CollectCryptoKeyFragment(CollectCryptoKeyFragmentRequest request);
        IPromise<OpenStorageResponse> OpenStorage();
        IPromise<UseSpeedRocketBoosterResponse> UseSpeedRocketBooster();
        IPromise<UseFreezeTimeBoosterResponse> UseFreezeTimeBooster();

        IPromise<LeaderboardDataResponse> GetLeaderboardData();

        IPromise<ShopDataResponse> GetShopData();
        IPromise<BuyResponse> Buy(BuyRequest request);
    }
}
