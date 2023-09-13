using CryptoMaze.Client;
using CryptoMaze.ClientServer.Game.Responses;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using CryptoMaze.Common;
using RSG;
using CryptoMaze.ClientServer.Game.DataContainers;

public static class SceneHelper
{
    public static CryptoMazeClient CreateAndValidateClient()
    {
        var accessTokenExpirationDate = PlayerPrefsExt.GetDateTime("accessTokenExpirationDate");
        var refreshTokenExpirationDate = PlayerPrefsExt.GetDateTime("refreshTokenExpirationDate");

        if (!accessTokenExpirationDate.HasValue &&
            (!refreshTokenExpirationDate.HasValue || refreshTokenExpirationDate < DateTime.UtcNow))
        {
            SceneManager.LoadScene(0);
        }

        var cryptoMazeClient = new CryptoMazeClient(
            ServerUrlUtil.IdentityServerUrl,
            ServerUrlUtil.GameServerUrl,
            new CryptoMazeClientOptions(
                PlayerPrefs.GetString("accessToken"),
                PlayerPrefs.GetString("refreshToken"),
                accessTokenExpirationDate,
                refreshTokenExpirationDate));

        return cryptoMazeClient;
    }

    public static IPromise<PlayerDataResponse> GetAndUpdatePlayerData(CryptoMazeClient client, ResourcePanel resourcePanel, BoostersPanel boostersPanel)
    {
        // need to add waiting screen while waiting GetPlayerData()
        return client.Game
                .GetPlayerData()
                .Then(result =>
                {
                    try
                    {
                        foreach (var item in result.items)
                        {
                            UpdateItemData(item, resourcePanel, boostersPanel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }

                    return result;
                })
                .Catch(ex =>
                {
                    Debug.LogError(ex.ToString());

                    PlayerPrefs.SetString("accessToken", string.Empty);
                    PlayerPrefs.SetString("refreshToken", string.Empty);

                    SceneManager.LoadScene(0);

                    return null;
                });
    }

    public static IPromise<PlayerDataResponse> GetAndUpdatePlayerData(CryptoMazeClient client, InventoryPanel inventoryPanel)
    {
        // need to add waiting screen while waiting GetPlayerData()
        return client.Game
                .GetPlayerData()
                .Then(result =>
                {
                    try
                    {
                        foreach (var item in result.items)
                        {
                            UpdateItemData(item, inventoryPanel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }

                    return result;
                })
                .Catch(ex =>
                {
                    Debug.LogError(ex.ToString());

                    PlayerPrefs.SetString("accessToken", string.Empty);
                    PlayerPrefs.SetString("refreshToken", string.Empty);

                    SceneManager.LoadScene(0);

                    return null;
                });
    }

    public static void UpdateItemData(UserItemModel item, InventoryPanel inventoryPanel)
    {
        switch (item.Type)
        {
            case ItemType.CryptoKey:
                inventoryPanel.SetCryptoKeys(item.amount);
                break;
            case ItemType.FreezeTimeBooster:
                inventoryPanel.SetTimeFreezeBoosters(item.amount);
                break;
            case ItemType.SpeedRocketBooster:
                inventoryPanel.SetSpeedRocketBoosters(item.amount);
                break;
            case ItemType.CryptoKeyFragment:
                inventoryPanel.SetCryptoKeyFragments(item.amount);
                break;
            case ItemType.Energy:
            case ItemType.Ton:
            case ItemType.Blc:
            case ItemType.BtcBlock:
            case ItemType.EthBlock:
            case ItemType.TonBlock:
                break;
            default:
                Debug.LogWarning($"ItemType {item.Type} not implemented");
                break;
        }
    }

    public static void UpdateItemData(UserItemModel item, ResourcePanel resourcePanel, BoostersPanel boostersPanel)
    {
        switch (item.Type)
        {
            case ItemType.Energy:
                resourcePanel.SetEnergy(item.amount);
                break;
            case ItemType.Ton:
                resourcePanel.SetTonBalance(item.amount);
                break;
            case ItemType.Blc:
                resourcePanel.SetBlcBalance(item.amount);
                break;
            case ItemType.CryptoKey:
                boostersPanel.SetCryptoKeys(item.amount);
                break;
            case ItemType.FreezeTimeBooster:
                boostersPanel.SetFreezeTimeBoosters(item.amount);
                break;
            case ItemType.SpeedRocketBooster:
                boostersPanel.SetSpeedRocketBoosters(item.amount);
                break;
            case ItemType.CryptoKeyFragment:
                boostersPanel.SetCryptoKeyFragments(item.amount);
                break;
            case ItemType.BtcBlock:
                resourcePanel.SetBtcBlocks(item.amount);
                break;
            case ItemType.EthBlock:
                resourcePanel.SetEthBlocks(item.amount);
                break;
            case ItemType.TonBlock:
                resourcePanel.SetTonBlocks(item.amount);
                break;
            default:
                Debug.LogWarning($"ItemType {item.Type} not implemented");
                break;
        }
    }
}
