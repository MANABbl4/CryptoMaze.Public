namespace CryptoMaze.ClientServer.Game.Responses
{
    public class FinishLabirintResponse
    {
        public bool finished;
        public bool canStartNextLabirint;
        public string message;

        public int labirintBtcAmount;
        public int labirintEthAmount;
        public int labirintTonAmount;
        public int labirintEnergyAmount;
        public int labirintCryptoKeyFragments;
    }
}
