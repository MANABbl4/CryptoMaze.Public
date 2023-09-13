using CryptoMaze.Common;
using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class UserItemModel
    {
        public int type;
        public int amount;

        public ItemType Type
        {
            get { return (ItemType)type; }
            set { type = (int)value; }
        }
    }
}
