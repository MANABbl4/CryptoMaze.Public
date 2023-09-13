using CryptoMaze.ClientServer.Game.DataContainers;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class BuyResponse
    {
        public bool bought;
        public UserItemModel boughtItemUpdatedValue;
        public UserItemModel soldItemUpdatedValue;
    }
}
