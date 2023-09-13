using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class EnergyData
    {
        public string id;
        public bool found;
        public bool storage;
    }
}