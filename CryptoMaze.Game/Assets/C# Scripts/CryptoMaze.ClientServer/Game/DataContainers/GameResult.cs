using System;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class GameResult
    {
        public int rank;
        public int result;
        public string userId;
        public string userName;
        public UserItemModel[] reward;
    }
}
