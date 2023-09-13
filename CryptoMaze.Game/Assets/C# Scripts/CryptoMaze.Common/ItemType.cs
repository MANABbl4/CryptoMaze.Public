using System;

namespace CryptoMaze.Common
{
    [Serializable]
    public enum ItemType : int
    {
        Energy = 0,
        Ton = 1,
        Blc = 2,
        CryptoKey = 3,
        FreezeTimeBooster = 4,
        SpeedRocketBooster = 5,
        CryptoKeyFragment = 6,
        BtcBlock = 7,
        EthBlock = 8,
        TonBlock = 9,
    }
}
