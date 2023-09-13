using CryptoMaze.ClientServer.Game.DataContainers;

namespace CryptoMaze.ClientServer.Game.Responses
{
    public class LeaderboardDataResponse
    {
        public LeaderboardData leaderboard;
        public GameResult playerBtcResult;
        public GameResult playerEthResult;
        public GameResult playerTonResult;
    }
}
