using CryptoMaze.Common;
using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class CryptoBlockData
    {
        public string id;
        public int type;
        public bool found;
        public bool storage;

        public CryptoType Type
        {
            get { return (CryptoType)type; }
            set { type = (int)value; }
        }
    }
}
