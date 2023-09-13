using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class StartLabirintResponse
    {
        public bool started;
        public string message;
        public int labirintOrderId;
        public int timeToFinish;
        public bool hasCryptoStorage;
        public bool timeFreezed;
        public bool speedRocketActivated;
        public List<CryptoBlockData> cryptoBlocks;
        public List<EnergyData> energies;
        public List<CryptoKeyFragmentData> cryptoKeyFragments;
    }
}
