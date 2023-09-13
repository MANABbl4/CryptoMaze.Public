using CryptoMaze.Common;

namespace CryptoMaze.LabirintGameServer.BLL
{
    public class LeaderboardConfiguration
    {
        public class TopCountConfiguration
        {
            public class RawardConfiguration
            {
                public int Rank { get; private set; }
                public decimal RewardPart { get; private set; }
                public ItemType Currency { get; private set; }
            }

            public int PlayedUsersMaxCount { get; private set; }
            public int LeaderboardTopCount { get; private set; }

            public IEnumerable<RawardConfiguration> Rewards { get; private set; }
        }

        public IEnumerable<TopCountConfiguration> TopCounts { get; private set; }
    }
}
