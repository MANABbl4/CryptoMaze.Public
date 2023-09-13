namespace CryptoMaze.ClientServer.Game.Responses
{
    public class CollectCryptoBlockResponse
    {
        public bool Collected { get; set; }
        public string Message { get; set; }
        public Common.CryptoType CollectedType { get; set; }
        public int CollectedAmount { get; set; }
    }
}
