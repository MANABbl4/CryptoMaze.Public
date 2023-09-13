namespace CryptoMaze.ClientServer.Game.Responses
{
    public class FinishLabirintResponse
    {
        public bool Finished { get; set; }
        public bool CanStartNextLabirint { get; set; }
        public string Message { get; set; }

        public int LabirintBtcAmount { get; set; }
        public int LabirintEthAmount { get; set; }
        public int LabirintTonAmount { get; set; }
        public int LabirintEnergyAmount { get; set; }
        public int LabirintCryptoKeyFragments { get; set; }
    }
}
