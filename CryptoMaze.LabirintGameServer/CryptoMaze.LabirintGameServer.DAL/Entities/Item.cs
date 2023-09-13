using CryptoMaze.Common;

namespace CryptoMaze.LabirintGameServer.DAL.Entities
{
    public sealed class Item : IEntity<int>
    {
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }

        public ICollection<ShopProposal> BuyProposals { get; set; }
        public ICollection<ShopProposal> SellProposals { get; set; }
        public ICollection<UserItem> UserItems { get; set; }
    }
}
