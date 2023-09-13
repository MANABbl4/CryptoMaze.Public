using CryptoMaze.Common;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    public class ShopProposalModel
    {
        public int ProposalId { get; set; }
        public ItemType BuyType { get; set; }
        public ShopItemType BuyShopType { get; set; }
        public int BuyAmount { get; set; }
        public ItemType SellType { get; set; }
        public ShopItemType SellShopType { get; set; }
        public int SellAmount { get; set; }
        public int Order { get; set; }
    }
}
