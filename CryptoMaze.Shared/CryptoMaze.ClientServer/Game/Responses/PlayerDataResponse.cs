using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class PlayerDataResponse
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public bool NameChanged { get; set; }
        public int EnergyStartGameNeeded { get; set; }
        public bool LastGameFinished { get; set; }

        public IEnumerable<UserItemModel> Items { get; set; }
    }
}
