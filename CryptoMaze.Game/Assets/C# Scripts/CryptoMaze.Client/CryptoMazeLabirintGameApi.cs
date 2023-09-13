using CryptoMaze.ClientServer.Authentication.Responses;
using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.ClientServer.Game.Responses;
using RSG;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace CryptoMaze.Client
{
    public class CryptoMazeLabirintGameApi : CryptoMazeApiBase, ICryptoMazeLabirintGameClient
    {
        private readonly string _getPlayerDataUrl;
        private readonly string _setPlayerNameUrl;
        private readonly string _addEnergyUrl;

        private readonly string _startGameUrl;
        private readonly string _startLabirintUrl;
        private readonly string _finishLabirintUrl;
        private readonly string _finishGameUrl;
        private readonly string _collectCryptoBlockUrl;
        private readonly string _collectCryptoKeyFragmentUrl;
        private readonly string _collectEnergyUrl;
        private readonly string _openStorageUrl;
        private readonly string _useFreezeTimeBoosterUrl;
        private readonly string _useSpeedRocketBoosterUrl;

        private readonly string _getLeaderboardDataUrl;

        private readonly string _getShopDataUrl;
        private readonly string _buyUrl;

        public CryptoMazeLabirintGameApi(string serverUrl, CryptoMazeClientOptions options, Func<IPromise<LoginResponse>> refreshTokenFunc)
            : base(serverUrl, options, refreshTokenFunc)
        {
            _getPlayerDataUrl = ServerUrl + "/Player/GetData";
            _setPlayerNameUrl = ServerUrl + "/Player/SetName";
            _addEnergyUrl = ServerUrl + "/Player/AddEnergy";

            _startGameUrl = ServerUrl + "/Game/StartGame";
            _startLabirintUrl = ServerUrl + "/Game/StartLabirint";
            _finishLabirintUrl = ServerUrl + "/Game/FinishLabirint";
            _finishGameUrl = ServerUrl + "/Game/FinishGame";
            _collectCryptoBlockUrl = ServerUrl + "/Game/CollectCryptoBlock";
            _collectCryptoKeyFragmentUrl = ServerUrl + "/Game/CollectCryptoKeyFragment";
            _collectEnergyUrl = ServerUrl + "/Game/CollectEnergy";
            _openStorageUrl = ServerUrl + "/Game/OpenStorage";
            _useFreezeTimeBoosterUrl = ServerUrl + "/Game/UseFreezeTimeBooster";
            _useSpeedRocketBoosterUrl = ServerUrl + "/Game/UseSpeedRocketBooster";

            _getLeaderboardDataUrl = ServerUrl + "/Leaderboard/GetData";

            _getShopDataUrl = ServerUrl + "/Shop/GetData";
            _buyUrl = ServerUrl + "/Shop/Buy";
        }

        #region Player
        public IPromise<PlayerDataResponse> GetPlayerData()
        {
            return Get<PlayerDataResponse>(_getPlayerDataUrl);
        }

        public IPromise<SetNameResponse> SetPlayerName(SetNameRequest request)
        {
            return Post<SetNameResponse>(_setPlayerNameUrl, request);
        }

        public IPromise<AddEnergyResponse> AddEnergy()
        {
            return Post<AddEnergyResponse>(_addEnergyUrl, null);
        }
        #endregion

        #region Game
        public IPromise<StartGameResponse> StartGame()
        {
            return Post<StartGameResponse>(_startGameUrl, null);
        }

        public IPromise<StartLabirintResponse> StartLabirint()
        {
            return Post<StartLabirintResponse>(_startLabirintUrl, null);
        }

        public IPromise<FinishLabirintResponse> FinishLabirint()
        {
            return Post<FinishLabirintResponse>(_finishLabirintUrl, null);
        }

        public IPromise<FinishGameResponse> FinishGame()
        {
            return Post<FinishGameResponse>(_finishGameUrl, null);
        }

        public IPromise<CollectCryptoBlockResponse> CollectCryptoBlock(CollectCryptoBlockRequest request)
        {
            return Post<CollectCryptoBlockResponse>(_collectCryptoBlockUrl, request);
        }

        public IPromise<CollectCryptoKeyFragmentResponse> CollectCryptoKeyFragment(CollectCryptoKeyFragmentRequest request)
        {
            return Post<CollectCryptoKeyFragmentResponse>(_collectCryptoKeyFragmentUrl, request);
        }

        public IPromise<CollectEnergyResponse> CollectEnergy(CollectEnergyRequest request)
        {
            return Post<CollectEnergyResponse>(_collectEnergyUrl, request);
        }

        public IPromise<OpenStorageResponse> OpenStorage()
        {
            return Post<OpenStorageResponse>(_openStorageUrl, null);
        }

        public IPromise<UseFreezeTimeBoosterResponse> UseFreezeTimeBooster()
        {
            return Post<UseFreezeTimeBoosterResponse>(_useFreezeTimeBoosterUrl, null);
        }

        public IPromise<UseSpeedRocketBoosterResponse> UseSpeedRocketBooster()
        {
            return Post<UseSpeedRocketBoosterResponse>(_useSpeedRocketBoosterUrl, null);
        }
        #endregion

        #region Leaderboard
        public IPromise<LeaderboardDataResponse> GetLeaderboardData()
        {
            return Get<LeaderboardDataResponse>(_getLeaderboardDataUrl);
        }
        #endregion

        #region Shop
        public IPromise<ShopDataResponse> GetShopData()
        {
            return Get<ShopDataResponse>(_getShopDataUrl);
        }

        public IPromise<BuyResponse> Buy(BuyRequest request)
        {
            return Post<BuyResponse>(_buyUrl, request);
        }
        #endregion
    }
}
