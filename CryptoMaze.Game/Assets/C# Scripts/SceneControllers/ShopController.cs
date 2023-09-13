using System;
using UnityEngine;
using CryptoMaze.Client;
using CryptoMaze.Common;
using CryptoMaze.ClientServer.Game.Responses;
using System.Linq;

public class ShopController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private ResourcePanel _resourcePanel;

    [SerializeField]
    private BoostersPanel _boostersPanel;

    [SerializeField]
    private ItemType[] _includeBuyTypes;

    [SerializeField]
    private ItemType[] _excludeSellTypes;

    [SerializeField]
    private ShopScrollView _scrollView;


    private CryptoMazeClient _cryptoMazeClient;
    private PlayerDataResponse _playerData = null;

    private void Start()
    {
        _cryptoMazeClient = SceneHelper.CreateAndValidateClient();

        SceneHelper.GetAndUpdatePlayerData(_cryptoMazeClient, _resourcePanel, _boostersPanel)
            .Then(playerData =>
            {
                _playerData = playerData;
            });

        _cryptoMazeClient.Game
            .GetShopData()
            .Then(shopData =>
            {
                var filteredItems = shopData.shopProposals
                    .Where(x => _includeBuyTypes != null && _includeBuyTypes.Contains(x.BuyType))
                    .Where(x => _excludeSellTypes != null && !_excludeSellTypes.Contains(x.SellType))
                    .ToList();

                _scrollView.OnBuyItem += OnBuy;
                _scrollView.SetProposals(filteredItems);
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnBuy(int proposalId)
    {
        _cryptoMazeClient.Game
            .Buy(new CryptoMaze.ClientServer.Game.Requests.BuyRequest() { proposalId = proposalId })
            .Then(buyResponse =>
            {
                if (buyResponse.bought)
                {
                    SceneHelper.UpdateItemData(buyResponse.boughtItemUpdatedValue, _resourcePanel, _boostersPanel);
                    SceneHelper.UpdateItemData(buyResponse.soldItemUpdatedValue, _resourcePanel, _boostersPanel);
                }
                else
                {
                    Debug.LogWarning($"Failed to buy proposalId {proposalId}");
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }
}
