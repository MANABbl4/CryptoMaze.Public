using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    public class CryptoKeyFragmentData
    {
        public Guid Id { get; set; }
        public bool Found { get; set; }
        public bool Storage { get; set; }
    }
}
