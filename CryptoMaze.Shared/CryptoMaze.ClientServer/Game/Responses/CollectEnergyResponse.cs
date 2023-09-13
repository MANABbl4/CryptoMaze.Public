namespace CryptoMaze.ClientServer.Game.Responses
{
    public class CollectEnergyResponse
    {
        public bool Collected { get; set; }
        public string Message { get; set; }
        public int CollectedAmount { get; set; }
    }
}
