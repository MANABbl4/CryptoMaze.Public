using CryptoMaze.Common;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class CollectCryptoBlockResponse
    {
        public bool collected;
        public string message;
        public int collectedType;
        public int collectedAmount;

        public CryptoType CollectedType
        {
            get { return (CryptoType)collectedType; }
            set { collectedType = (int)value; }
        }
    }
}
