using System;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    public class GameResult
    {
        public int Rank { get; set; }
        public int Result { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public List<UserItemModel> Reward { get; set; }
    }
}
