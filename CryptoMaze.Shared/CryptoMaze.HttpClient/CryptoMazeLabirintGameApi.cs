using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace CryptoMaze.Client
{
    public class CryptoMazeLabirintGameApi : CryptoMazeApiBase, ICryptoMazeLabirintGameClient
    {
        private readonly Uri _collectCryptoBlockUrl;
        private readonly Uri _collectCryptoKeyFragmentUrl;
        private readonly Uri _collectEnergyUrl;
        private readonly Uri _finishLabirintUrl;
        private readonly Uri _getPlayerDataUrl;
        private readonly Uri _openStorageUrl;
        private readonly Uri _setPlayerNameUrl;
        private readonly Uri _startGameUrl;
        private readonly Uri _useFreezeTimeBoosterUrl;
        private readonly Uri _useSpeedRocketBoosterUrl;
        private readonly Uri _getLeaderboardDataUrl;
        private readonly Uri _getShopDataUrl;
        private readonly Uri _buyUrl;

        public CryptoMazeLabirintGameApi(string serverUrl, CryptoMazeClientOptions options, Func<Task<WebCallResult<LoginResponse>>> refreshTokenFunc)
            : base(serverUrl, options, refreshTokenFunc)
        {
            _getPlayerDataUrl = new Uri(ServerUrl, new Uri("Player/GetData", UriKind.Relative));
            _setPlayerNameUrl = new Uri(ServerUrl, new Uri("Player/SetName", UriKind.Relative));
            _startGameUrl = new Uri(ServerUrl, new Uri("Game/Start", UriKind.Relative));
            _finishLabirintUrl = new Uri(ServerUrl, new Uri("Game/FinishLabirint", UriKind.Relative));
            _collectCryptoBlockUrl = new Uri(ServerUrl, new Uri("Game/CollectCryptoBlock", UriKind.Relative));
            _collectCryptoKeyFragmentUrl = new Uri(ServerUrl, new Uri("Game/CollectCryptoKeyFragment", UriKind.Relative));
            _collectEnergyUrl = new Uri(ServerUrl, new Uri("Game/CollectEnergy", UriKind.Relative));
            _openStorageUrl = new Uri(ServerUrl, new Uri("Game/OpenStorage", UriKind.Relative));
            _useFreezeTimeBoosterUrl = new Uri(ServerUrl, new Uri("Game/UseFreezeTimeBooster", UriKind.Relative));
            _useSpeedRocketBoosterUrl = new Uri(ServerUrl, new Uri("Game/UseSpeedRocketBooster", UriKind.Relative));
            _getLeaderboardDataUrl = new Uri(ServerUrl, new Uri("Leaderboard/GetData", UriKind.Relative));
            _getShopDataUrl = new Uri(ServerUrl, new Uri("Shop/GetData", UriKind.Relative));
            _buyUrl = new Uri(ServerUrl, new Uri("Shop/Buy", UriKind.Relative));
        }

        #region Player
        public Task<WebCallResult<PlayerDataResponse>> GetPlayerDataAsync()
        {
            return GetAsync<PlayerDataResponse>(_getPlayerDataUrl);
        }

        public Task<WebCallResult<SetNameResponse>> SetPlayerNameAsync(SetNameRequest request)
        {
            return PostAsync<SetNameResponse>(_setPlayerNameUrl, JsonConvert.SerializeObject(request));
        }

        public Task<WebCallResult<AddEnergyResponse>> AddEnergyAsync()
        {
            return PostAsync<AddEnergyResponse>(_setPlayerNameUrl, string.Empty);
        }
        #endregion

        #region Game
        public Task<WebCallResult<StartGameResponse>> StartGameAsync()
        {
            return PostAsync<StartGameResponse>(_startGameUrl, string.Empty);
        }

        public Task<WebCallResult<FinishLabirintResponse>> FinishLabirintAsync()
        {
            return PostAsync<FinishLabirintResponse>(_finishLabirintUrl, string.Empty);
        }

        public Task<WebCallResult<CollectCryptoBlockResponse>> CollectCryptoBlockAsync(CollectCryptoBlockRequest request)
        {
            return PostAsync<CollectCryptoBlockResponse>(_collectCryptoBlockUrl, JsonConvert.SerializeObject(request));
        }

        public Task<WebCallResult<CollectCryptoKeyFragmentResponse>> CollectCryptoKeyFragmentAsync(CollectCryptoKeyFragmentRequest request)
        {
            return PostAsync<CollectCryptoKeyFragmentResponse>(_collectCryptoKeyFragmentUrl, JsonConvert.SerializeObject(request));
        }

        public Task<WebCallResult<CollectEnergyResponse>> CollectEnergyAsync(CollectEnergyRequest request)
        {
            return PostAsync<CollectEnergyResponse>(_collectEnergyUrl, JsonConvert.SerializeObject(request));
        }

        public Task<WebCallResult<OpenStorageResponse>> OpenStorageAsync()
        {
            return PostAsync<OpenStorageResponse>(_openStorageUrl, string.Empty);
        }

        public Task<WebCallResult<UseFreezeTimeBoosterResponse>> UseFreezeTimeBoosterAsync()
        {
            return PostAsync<UseFreezeTimeBoosterResponse>(_useFreezeTimeBoosterUrl, string.Empty);
        }

        public Task<WebCallResult<UseSpeedRocketBoosterResponse>> UseSpeedRocketBoosterAsync()
        {
            return PostAsync<UseSpeedRocketBoosterResponse>(_useSpeedRocketBoosterUrl, string.Empty);
        }
        #endregion

        #region Leaderboard
        public Task<WebCallResult<LeaderboardDataResponse>> GetLeaderboardDataAsync()
        {
            return GetAsync<LeaderboardDataResponse>(_getLeaderboardDataUrl);
        }
        #endregion

        #region Shop
        public Task<WebCallResult<ShopDataResponse>> GetShopDataAsync()
        {
            return GetAsync<ShopDataResponse>(_getShopDataUrl);
        }

        public Task<WebCallResult<BuyResponse>> BuyAsync(BuyRequest request)
        {
            return PostAsync<BuyResponse>(_buyUrl, JsonConvert.SerializeObject(request));
        }
        #endregion
    }
}
