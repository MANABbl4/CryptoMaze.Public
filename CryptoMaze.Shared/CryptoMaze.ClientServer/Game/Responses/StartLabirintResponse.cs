using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class StartLabirintResponse
    {
        public bool Started { get; set; }
        public string Message { get; set; }
        public int LabirintOrderId { get; set; }
        public int TimeToFinish { get; set; }
        public bool HasCryptoStorage { get; set; }
        public bool TimeFreezed { get; set; }
        public bool SpeedRocketActivated { get; set; }
        public IEnumerable<CryptoBlockData> CryptoBlocks { get; set; }
        public IEnumerable<EnergyData> Energies { get; set; }
        public IEnumerable<CryptoKeyFragmentData> CryptoKeyFragments { get; set; }
    }
}
