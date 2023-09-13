using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class PlayerDataResponse
    {
        public string email;
        public string name;
        public bool nameChanged;
        public int energyStartGameNeeded;
        public bool lastGameFinished;

        public List<UserItemModel> items;
    }
}
