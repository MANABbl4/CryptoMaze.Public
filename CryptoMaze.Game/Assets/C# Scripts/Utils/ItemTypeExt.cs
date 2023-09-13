using CryptoMaze.Common;
using System;

public static class ItemTypeExt
{
    public static string GetUIName(this ItemType type)
    {
        switch (type)
        {
            case ItemType.Energy:
                return "Energy";
            case ItemType.Ton:
                return "TON";
            case ItemType.Blc:
                return "BLC";
            case ItemType.CryptoKey:
                return "Crypto Key";
            case ItemType.FreezeTimeBooster:
                return "Time Freeze";
            case ItemType.SpeedRocketBooster:
                return "Speed Rocket";
            case ItemType.CryptoKeyFragment:
                return "Key Fragment";
            case ItemType.BtcBlock:
                return "BTC Block";
            case ItemType.EthBlock:
                return "ETH Block";
            case ItemType.TonBlock:
                return "TON Block";
            default:
                throw new NotImplementedException($"Not implemented type {type}.");
        }
    }
}
