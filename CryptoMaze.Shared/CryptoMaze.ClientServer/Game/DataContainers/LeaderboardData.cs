using System;
using System.Collections.Generic;

namespace CryptoMaze.ClientServer.Game.DataContainers
{
    public class LeaderboardData
    {
        public int SeasonNumber { get; set; }
        public DateTime FinishDate { get; set; }
        public int TimeLeft { get; set; }
        public int TopCount { get; set; }
        public IEnumerable<GameResult> BtcTopResults { get; set; }
        public IEnumerable<GameResult> EthTopResults { get; set; }
        public IEnumerable<GameResult> TonTopResults { get; set; }
    }
}
