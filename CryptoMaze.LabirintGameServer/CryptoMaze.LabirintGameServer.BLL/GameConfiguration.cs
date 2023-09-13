namespace CryptoMaze.LabirintGameServer.BLL
{
    public class GameConfiguration
    {
        public IEnumerable<LabirintConfiguration> Labirints { get; private set; }
        public int BtcBlockMinAmount { get; private set; }
        public int BtcBlockMaxAmount { get; private set; }
        public int EthBlockMinAmount { get; private set; }
        public int EthBlockMaxAmount { get; private set; }
        public int TonBlockMinAmount { get; private set; }
        public int TonBlockMaxAmount { get; private set; }
        public int EnergyMinAmount { get; private set; }
        public int EnergyMaxAmount { get; private set; }
        public int CryptoKeyFragmentMinAmount { get; private set; }
        public int CryptoKeyFragmentMaxAmount { get; private set; }
        public int EnergyStartGameNeeded { get; private set; }
    }
}
