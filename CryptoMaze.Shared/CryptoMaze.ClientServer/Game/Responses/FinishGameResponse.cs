namespace CryptoMaze.ClientServer.Game.Responses
{
    public class FinishGameResponse
    {
        public bool Finished { get; set; }
        public string Message { get; set; }

        public int TotalBtcAmount { get; set; }
        public int TotalEthAmount { get; set; }
        public int TotalTonAmount { get; set; }
        public int TotalEnergyAmount { get; set; }
        public int TotalCryptoKeyFragments { get; set; }
    }
}
