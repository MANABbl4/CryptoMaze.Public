using CryptoMaze.Common;
using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class ShopProposalModel
    {
        public int proposalId;
        public int buyType;
        public int buyAmount;
        public int buyShopType;
        public int sellType;
        public int sellAmount;
        public int sellShopType;
        public int order;

        public ItemType BuyType
        {
            get { return (ItemType)buyType; }
            set { buyType = (int)value; }
        }

        public ShopItemType BuyShopType
        {
            get { return (ShopItemType)buyShopType; }
            set { buyShopType = (int)value; }
        }

        public ItemType SellType
        {
            get { return (ItemType)sellType; }
            set { sellType = (int)value; }
        }

        public ShopItemType SellShopType
        {
            get { return (ShopItemType)sellShopType; }
            set { sellShopType = (int)value; }
        }
    }
}
