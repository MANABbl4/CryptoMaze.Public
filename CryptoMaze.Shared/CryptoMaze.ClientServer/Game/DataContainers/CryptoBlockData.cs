using CryptoMaze.Common;
using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    public class CryptoBlockData
    {
        public Guid Id { get; set; }
        public CryptoType Type { get; set; }
        public bool Found { get; set; }
        public bool Storage { get; set; }
    }
}
