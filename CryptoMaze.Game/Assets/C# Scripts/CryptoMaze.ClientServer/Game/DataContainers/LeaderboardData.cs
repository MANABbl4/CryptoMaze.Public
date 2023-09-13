using System;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    [Serializable]
    public class LeaderboardData
    {
        public int seasonNumber;
        public int timeLeft;
        public int topCount;
        public List<GameResult> btcTopResults;
        public List<GameResult> ethTopResults;
        public List<GameResult> tonTopResults;
    }
}
