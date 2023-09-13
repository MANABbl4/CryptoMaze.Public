using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.Common;
using CryptoMaze.LabirintGameServer.BLL.Models;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Repositories;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly ISeasonRepository _seasonRepository;
        private readonly IShopProposalRepository _shopProposalRepository;
        private readonly IGenericRepository<SeasonHistory, int> _seasonHistoryRepository;

        private readonly LeaderboardConfiguration _leaderboardConfiguration;

        public LeaderboardService(IUserRepository userRepository, IGameRepository gameRepository,
            ISeasonRepository seasonRepository, IShopProposalRepository shopProposalRepository,
            IGenericRepository<SeasonHistory, int> seasonHistoryRepository,
            LeaderboardConfiguration leaderboardConfiguration)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _seasonRepository = seasonRepository;
            _shopProposalRepository = shopProposalRepository;
            _seasonHistoryRepository = seasonHistoryRepository;
            _leaderboardConfiguration = leaderboardConfiguration;
        }

        public async Task<LeaderboardDataResponse> GetDataAsync(string email, LeaderboardData leaderboardData)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new LeaderboardDataResponse()
                {
                };
            }

            var topCount = _leaderboardConfiguration.TopCounts
                .OrderBy(x => x.PlayedUsersMaxCount)
                .FirstOrDefault(x => leaderboardData.BtcTopResults.Count() < x.PlayedUsersMaxCount);

            var result = new LeaderboardDataResponse()
            {
                Leaderboard = new LeaderboardData()
                {
                    SeasonNumber = leaderboardData.SeasonNumber,
                    FinishDate = leaderboardData.FinishDate,
                    TimeLeft = (int)(leaderboardData.FinishDate - DateTime.UtcNow).TotalSeconds,
                    TopCount = topCount.Rewards.Count(),
                    BtcTopResults = leaderboardData.BtcTopResults.Take(topCount.LeaderboardTopCount),
                    EthTopResults = leaderboardData.EthTopResults.Take(topCount.LeaderboardTopCount),
                    TonTopResults = leaderboardData.TonTopResults.Take(topCount.LeaderboardTopCount),
                },
                PlayerBtcResult = leaderboardData.BtcTopResults.FirstOrDefault(x => x.UserId == user.Id),
                PlayerEthResult = leaderboardData.EthTopResults.FirstOrDefault(x => x.UserId == user.Id),
                PlayerTonResult = leaderboardData.TonTopResults.FirstOrDefault(x => x.UserId == user.Id)
            };

            if (result.PlayerBtcResult == null)
            {
                result.PlayerBtcResult = new GameResult()
                {
                    Rank = leaderboardData.BtcTopResults.Count() + 1,
                    Result = 0,
                    UserId = user.Id,
                    UserName = user.Name
                };
            }

            if (result.PlayerEthResult == null)
            {
                result.PlayerEthResult = new GameResult()
                {
                    Rank = leaderboardData.EthTopResults.Count() + 1,
                    Result = 0,
                    UserId = user.Id,
                    UserName = user.Name
                };
            }

            if (result.PlayerTonResult == null)
            {
                result.PlayerTonResult = new GameResult()
                {
                    Rank = leaderboardData.TonTopResults.Count() + 1,
                    Result = 0,
                    UserId = user.Id,
                    UserName = user.Name
                };
            }

            return result;
        }

        public async Task<LeaderboardData> GetCurrentSeasonDataAsync()
        {
            var season = await _seasonRepository.GetSeasonAsync(DateTime.UtcNow);

            return await GetSeasonDataAsync(season);
        }

        public async Task<FinishSeasonResponse> FinishSeasonAsync()
        {
            var season = await _seasonRepository.GetPreviousSeasonAsync(DateTime.UtcNow);

            if (season == null || season.Finished)
            {
                return new FinishSeasonResponse()
                {
                    Finished = false
                };
            }

            var leaderboardData = await GetSeasonDataAsync(season);

            var results = leaderboardData.BtcTopResults
                .Select(x => new SeasonHistory()
                {
                    Rank = x.Rank,
                    Score = x.Result,
                    Type = Common.CryptoType.Btc,
                    UserName = x.UserName,
                    UserId = x.UserId,
                    SeasonId = season.Id
                })
                .ToList();

            results.AddRange(leaderboardData.EthTopResults
                .Select(x => new SeasonHistory()
                {
                    Rank = x.Rank,
                    Score = x.Result,
                    Type = Common.CryptoType.Eth,
                    UserName = x.UserName,
                    UserId = x.UserId,
                    SeasonId = season.Id
                })
                .ToList());

            results.AddRange(leaderboardData.TonTopResults
                .Select(x => new SeasonHistory()
                {
                    Rank = x.Rank,
                    Score = x.Result,
                    Type = Common.CryptoType.Ton,
                    UserName = x.UserName,
                    UserId = x.UserId,
                    SeasonId = season.Id
                })
                .ToList());

            season.Finished = true;

            await _seasonHistoryRepository.AddRangeAsync(results);

            await _seasonRepository.UpdateAsync(season);

            return new FinishSeasonResponse()
            {
                Finished = true,
                SeasonNumber = season.Number
            };
        }

        private async Task<LeaderboardData> GetSeasonDataAsync(Season season)
        {
            var games = await _gameRepository.GetGameResults(season.StartDate, season.FinishDate);
            var shopItems = await _shopProposalRepository.GetAsync();

            var btcResults = games
                .GroupBy(x => x.UserId)
                .Select(g => g.OrderByDescending(x => x.BtcBlocksCollected).First())
                .OrderByDescending(x => x.BtcBlocksCollected)
                .ThenBy(x => x.StartTime)
                .Select((x, i) => new GameResult()
                {
                    Rank = i + 1,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    Result = x.BtcBlocksCollected, 
                    Reward = new List<UserItemModel>()
                })
                .ToList();

            var ethResults = games
                .GroupBy(x => x.UserId)
                .Select(g => g.OrderByDescending(x => x.EthBlocksCollected).First())
                .OrderByDescending(x => x.EthBlocksCollected)
                .ThenBy(x => x.StartTime)
                .Select((x, i) => new GameResult()
                {
                    Rank = i + 1,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    Result = x.EthBlocksCollected,
                    Reward = new List<UserItemModel>()
                })
                .ToList();

            var tonResults = games
                .GroupBy(x => x.UserId)
                .Select(g => g.OrderByDescending(x => x.TonBlocksCollected).First())
                .OrderByDescending(x => x.TonBlocksCollected)
                .ThenBy(x => x.StartTime)
                .Select((x, i) => new GameResult()
                {
                    Rank = i + 1,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    Result = x.TonBlocksCollected,
                    Reward = new List<UserItemModel>()
                })
                .ToList();

            var totalEnergySpent = games.Sum(x => x.EnergySpent);
            var tonBlcRate = shopItems.First(x => x.BuyItem.Type == ItemType.Blc && x.SellItem.Type == ItemType.Ton).BuyItemAmount;
            var energyBlcItem = shopItems.First(x => x.BuyShopItemType == ShopItemType.Energy && x.SellShopItemType == ShopItemType.Blc);
            var totalBlcAmount = totalEnergySpent * energyBlcItem.SellItemAmount / energyBlcItem.BuyItemAmount;
            var disciplineBlcAmount = totalBlcAmount / 3;

            var topCount = _leaderboardConfiguration.TopCounts
                .OrderBy(x => x.PlayedUsersMaxCount)
                .FirstOrDefault(x => games.Count() < x.PlayedUsersMaxCount);

            int i = 0;

            foreach (var reward in topCount.Rewards)
            {
                if (i < btcResults.Count)
                {
                    int amount = 0;
                    if (reward.Currency == ItemType.Ton)
                    {
                        amount = (int)(disciplineBlcAmount * reward.RewardPart / tonBlcRate);
                    }
                    else
                    {
                        amount = (int)(disciplineBlcAmount * reward.RewardPart);
                    }

                    btcResults[i].Reward.Add(new UserItemModel() { Type = reward.Currency, Amount = amount });
                    ethResults[i].Reward.Add(new UserItemModel() { Type = reward.Currency, Amount = amount });
                    tonResults[i].Reward.Add(new UserItemModel() { Type = reward.Currency, Amount = amount });
                }

                ++i;
            }

            for (; i < btcResults.Count; ++i)
            {
                var boostersAmount = GetBoostersAmount(btcResults[i].Rank - topCount.Rewards.Count(), btcResults.Count);

                btcResults[i].Reward.Add(new UserItemModel() { Type = ItemType.CryptoKey, Amount = boostersAmount });
                btcResults[i].Reward.Add(new UserItemModel() { Type = ItemType.SpeedRocketBooster, Amount = boostersAmount });
                btcResults[i].Reward.Add(new UserItemModel() { Type = ItemType.FreezeTimeBooster, Amount = boostersAmount });

                ethResults[i].Reward.Add(new UserItemModel() { Type = ItemType.CryptoKey, Amount = boostersAmount });
                ethResults[i].Reward.Add(new UserItemModel() { Type = ItemType.SpeedRocketBooster, Amount = boostersAmount });
                ethResults[i].Reward.Add(new UserItemModel() { Type = ItemType.FreezeTimeBooster, Amount = boostersAmount });

                tonResults[i].Reward.Add(new UserItemModel() { Type = ItemType.CryptoKey, Amount = boostersAmount });
                tonResults[i].Reward.Add(new UserItemModel() { Type = ItemType.SpeedRocketBooster, Amount = boostersAmount });
                tonResults[i].Reward.Add(new UserItemModel() { Type = ItemType.FreezeTimeBooster, Amount = boostersAmount });
            }

            return new LeaderboardData()
            {
                SeasonNumber = season.Number,
                FinishDate = season.FinishDate,
                BtcTopResults = btcResults,
                EthTopResults = ethResults,
                TonTopResults = tonResults,
            };
        }

        private int GetBoostersAmount(int rank, int size)
        {
            double a = 0.00000142874022;
            double b = -0.000216716898;
            double c = 0.0124829253;
            double d = -0.440801156;
            double e = 10.0045884;
            double defaultSize = 55;

            double rank1 = rank * defaultSize / size;
            double rank2 = rank1 * rank1;
            double rank3 = rank2 * rank1;
            double rank4 = rank2 * rank2;

            return (int)(a * rank4 + b * rank3 + c * rank2 + d * rank1 + e);
        }
    }
}
