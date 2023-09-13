using CryptoMaze.ClientServer.Game.DataContainers;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class BuyResponse
    {
        public bool Bought { get; set; }
        public UserItemModel BoughtItemUpdatedValue { get; set; }
        public UserItemModel SoldItemUpdatedValue { get; set; }
    }
}
