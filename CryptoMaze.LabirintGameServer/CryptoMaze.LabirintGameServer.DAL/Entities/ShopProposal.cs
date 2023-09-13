using CryptoMaze.Common;

namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class ShopProposal : IEntity<int>
    {
        public int Id { get; set; }
        public int BuyItemId { get; set; }
        public Item BuyItem { get; set; }
        public ShopItemType BuyShopItemType { get; set; }
        public int BuyItemAmount { get; set; }
        public int SellItemId { get; set; }
        public Item SellItem { get; set; }
        public int SellItemAmount { get; set; }
        public ShopItemType SellShopItemType { get; set; }
        public int Order { get; set; }
    }
}
