namespace CryptoMaze.LabirintGameServer.BLL
{
    public class LabirintConfiguration
    {
        public int OrderId { get; private set; }
        public int MaxTimeSeconds { get; private set; }
        public int BtcBlocksSpawnsCount { get; private set; }
        public int EthBlocksSpawnsCount { get; private set; }
        public int TonBlocksSpawnsCount { get; private set; }
        public int EnergySpawnsCount { get; private set; }
        public int CryptoKeyFragmentsSpawnsCount { get; private set; }
        public float EnergyProbability { get; private set; }
        public float CryptoKeyFragmentProbability { get; private set; }

        public bool HasCryptoStorage { get; private set; }
        public int StorageBtcBlocksSpawnsCount { get; private set; }
        public int StorageEthBlocksSpawnsCount { get; private set; }
        public int StorageTonBlocksSpawnsCount { get; private set; }
        public int StorageEnergySpawnsCount { get; private set; }
        public int StorageCryptoKeyFragmentsSpawnsCount { get; private set; }
        public float StorageBtcBlockProbability { get; private set; }
        public float StorageEthBlockProbability { get; private set; }
        public float StorageTonBlockProbability { get; private set; }
        public float StorageEnergyProbability { get; private set; }
        public float StorageCryptoKeyFragmentProbability { get; private set; }

    }
}
