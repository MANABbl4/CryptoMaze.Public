using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class CryptoKeyFragmentData
    {
        public string id;
        public bool found;
        public bool storage;
    }
}
