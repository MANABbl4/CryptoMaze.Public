using CryptoMaze.ClientServer.Game.DataContainers;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class LeaderboardDataResponse
    {
        public LeaderboardData Leaderboard { get; set; }
        public GameResult PlayerBtcResult { get; set; }
        public GameResult PlayerEthResult { get; set; }
        public GameResult PlayerTonResult { get; set; }
    }
}
