namespace CryptoMaze.ClientServer.Game.Responses
{
    public class FinishGameResponse
    {
        public bool finished;
        public string message;
        public int totalBtcAmount;
        public int totalEthAmount;
        public int totalTonAmount;
        public int totalEnergyAmount;
        public int totalCryptoKeyFragments;
    }
}
