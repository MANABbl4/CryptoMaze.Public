using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.Common;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Repositories;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGenericRepository<Labirint, int> _labirintRepository;
        private readonly IUserItemRepository _userItemRepository;
        private readonly IGenericRepository<LabirintCryptoBlock, Guid> _labirintCryptoBlockRepository;
        private readonly IGenericRepository<LabirintEnergy, Guid> _labirintEnergyRepository;
        private readonly IGenericRepository<LabirintCryptoKeyFragment, Guid> _labirintLabirintCryptoKeyFragmentRepository;
        private readonly IGenericRepository<Item, int> _itemRepository;

        private readonly GameConfiguration _gameConfiguration;
        private readonly Random _random = new Random();

        public GameService(IUserRepository userRepository, IGameRepository gameRepository,
            IGenericRepository<Labirint, int> labirintRepository, IUserItemRepository userItemRepository,
            IGenericRepository<LabirintCryptoBlock, Guid> labirintCryptoBlockRepository,
            IGenericRepository<LabirintEnergy, Guid> labirintEnergyRepository,
            IGenericRepository<LabirintCryptoKeyFragment, Guid> labirintLabirintCryptoKeyFragmentRepository,
            IGenericRepository<Item, int> itemRepository, GameConfiguration gameConfiguration)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _labirintRepository = labirintRepository;
            _userItemRepository = userItemRepository;
            _labirintCryptoBlockRepository = labirintCryptoBlockRepository;
            _labirintEnergyRepository = labirintEnergyRepository;
            _labirintLabirintCryptoKeyFragmentRepository = labirintLabirintCryptoKeyFragmentRepository;
            _itemRepository = itemRepository;
            _gameConfiguration = gameConfiguration;
        }

        public async Task<StartGameResponse> StartGameAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame != null && !lastGame.FinishTime.HasValue)
            {
                // continue game anyway
                return new StartGameResponse()
                {
                    Started = true,
                    EnergySpent = 0,
                    Message = "Continue game, please"
                };
            }

            var userEnergy = await _userItemRepository.GetUserItem(email, Common.ItemType.Energy);

            if (userEnergy == null || userEnergy.ItemAmount < _gameConfiguration.EnergyStartGameNeeded)
            {
                return new StartGameResponse()
                {
                    Started = false,
                    EnergySpent = 0,
                    Message = "Not enough energy."
                };
            }

            userEnergy.ItemAmount -= _gameConfiguration.EnergyStartGameNeeded;
            await _userItemRepository.UpdateAsync(userEnergy);

            var game = new Game()
            {
                StartTime = DateTime.UtcNow,
                EnergySpent = _gameConfiguration.EnergyStartGameNeeded,
                FinishTime = null,
                BtcBlocksCollected = 0,
                EthBlocksCollected = 0,
                TonBlocksCollected = 0,
                TimeFreezeUsed = false,
                CryptoKeyUsed = false,
                UserId = userEnergy.UserId,
                User = userEnergy.User
            };

            await _gameRepository.AddAsync(game);

            var labirints = new List<Labirint>(_gameConfiguration.Labirints.Count());

            foreach (var labirintConfig in _gameConfiguration.Labirints.OrderBy(x => x.OrderId))
            {
                var labirint = new Labirint()
                {
                    LabirintOrderId = labirintConfig.OrderId,
                    StartTime = null,
                    FinishTime = null,
                    TimeToFinishSeconds = labirintConfig.MaxTimeSeconds,
                    HasCryptoStorage = labirintConfig.HasCryptoStorage,
                    SpeedRocketUsed = false,
                    GameId = game.Id,
                    Game = game
                };

                labirints.Add(labirint);
            }
            await _labirintRepository.AddRangeAsync(labirints);

            int counter = 0;
            var cryptoBlocks = new List<LabirintCryptoBlock>();
            var energies = new List<LabirintEnergy>();
            var cryptoKeyFragments = new List<LabirintCryptoKeyFragment>();

            foreach (var labirintConfig in _gameConfiguration.Labirints.OrderBy(x => x.OrderId))
            {
                var labirint = labirints[counter];
                ++counter;

                cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Btc, labirintConfig.BtcBlocksSpawnsCount, 1f, false));
                cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Eth, labirintConfig.EthBlocksSpawnsCount, 1f, false));
                cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Ton, labirintConfig.TonBlocksSpawnsCount, 1f, false));
                energies.AddRange(GenerateEnergies(labirint, labirintConfig.EnergySpawnsCount, labirintConfig.EnergyProbability, false));
                cryptoKeyFragments.AddRange(GenerateCryptoKeyFragments(labirint, labirintConfig.CryptoKeyFragmentsSpawnsCount, labirintConfig.CryptoKeyFragmentProbability, false));

                if (labirintConfig.HasCryptoStorage)
                {
                    cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Btc, labirintConfig.StorageBtcBlocksSpawnsCount, labirintConfig.StorageBtcBlockProbability, true));
                    cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Eth, labirintConfig.StorageEthBlocksSpawnsCount, labirintConfig.StorageEthBlockProbability, true));
                    cryptoBlocks.AddRange(GenerateCryptoBlocks(labirint, Common.CryptoType.Ton, labirintConfig.StorageTonBlocksSpawnsCount, labirintConfig.StorageTonBlockProbability, true));
                    energies.AddRange(GenerateEnergies(labirint, labirintConfig.StorageEnergySpawnsCount, labirintConfig.StorageEnergyProbability, true));
                    cryptoKeyFragments.AddRange(GenerateCryptoKeyFragments(labirint, labirintConfig.StorageCryptoKeyFragmentsSpawnsCount, labirintConfig.StorageCryptoKeyFragmentProbability, true));
                }
            }

            if (cryptoBlocks.Any())
            {
                await _labirintCryptoBlockRepository.AddRangeAsync(cryptoBlocks);
            }

            if (energies.Any())
            {
                await _labirintEnergyRepository.AddRangeAsync(energies);
            }

            if (cryptoKeyFragments.Any())
            {
                await _labirintLabirintCryptoKeyFragmentRepository.AddRangeAsync(cryptoKeyFragments);
            }

            return new StartGameResponse()
            {
                Started = true,
                EnergySpent = _gameConfiguration.EnergyStartGameNeeded
            };
        }

        public async Task<StartLabirintResponse> StartLabirintAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new StartLabirintResponse()
                {
                    Started = false,
                    Message = "Game hasn't started yet. Please, start new game first."
                };
            }

            var unfinishedLabirint = lastGame.Labirints
                .OrderBy(x => x.LabirintOrderId)
                .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (unfinishedLabirint != null)
            {
                if (lastGame.TimeFreezeUsed || (DateTime.UtcNow - unfinishedLabirint.StartTime.Value).TotalSeconds < unfinishedLabirint.TimeToFinishSeconds)
                {
                    // continue
                    return new StartLabirintResponse()
                    {
                        Started = true,
                        LabirintOrderId = unfinishedLabirint.LabirintOrderId,
                        TimeToFinish = unfinishedLabirint.TimeToFinishSeconds - (int)(DateTime.UtcNow - unfinishedLabirint.StartTime.Value).TotalSeconds,
                        CryptoBlocks = unfinishedLabirint.CryptoBlocks.Select(x => new CryptoBlockData() { Id = x.Id, Type = x.Type, Found = x.Found }).ToList(),
                        Energies = unfinishedLabirint.Energies.Select(x => new EnergyData() { Id = x.Id, Found = x.Found }).ToList(),
                        CryptoKeyFragments = unfinishedLabirint.CryptoKeyFragments.Select(x => new CryptoKeyFragmentData() { Id = x.Id, Found = x.Found }).ToList(),
                        HasCryptoStorage = unfinishedLabirint.HasCryptoStorage,
                        TimeFreezed = lastGame.TimeFreezeUsed,
                        SpeedRocketActivated = unfinishedLabirint.SpeedRocketUsed
                    };
                }
                else
                {
                    // timeout: finish labirint and game
                    return new StartLabirintResponse()
                    {
                        Started = false,
                        Message = "Timeout. Finish labirint, please."
                    };
                }
            }

            var curLabirint = lastGame.Labirints
                .OrderBy(x => x.LabirintOrderId)
                .FirstOrDefault(x => !x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new StartLabirintResponse()
                {
                    Started = false,
                    Message = " Finish game, please."
                };
            }

            curLabirint.StartTime = DateTime.UtcNow;
            await _labirintRepository.UpdateAsync(curLabirint);

            return new StartLabirintResponse()
            {
                Started = true,
                LabirintOrderId = curLabirint.LabirintOrderId,
                TimeToFinish = curLabirint.TimeToFinishSeconds,
                CryptoBlocks = curLabirint.CryptoBlocks.Select(x => new CryptoBlockData() { Id = x.Id, Type = x.Type, Found = x.Found, Storage = x.Storage }).ToList(),
                Energies = curLabirint.Energies.Select(x => new EnergyData() { Id = x.Id, Found = x.Found, Storage = x.Storage }).ToList(),
                CryptoKeyFragments = curLabirint.CryptoKeyFragments.Select(x => new CryptoKeyFragmentData() { Id = x.Id, Found = x.Found, Storage = x.Storage }).ToList(),
                HasCryptoStorage = curLabirint.HasCryptoStorage,
                TimeFreezed = lastGame.TimeFreezeUsed,
                SpeedRocketActivated = curLabirint.SpeedRocketUsed
            };
        }

        public async Task<FinishLabirintResponse> FinishLabirintAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new FinishLabirintResponse()
                {
                    Finished = true,
                    CanStartNextLabirint = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);
            var nextLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => !x.StartTime.HasValue);

            if (curLabirint == null)
            {
                return new FinishLabirintResponse()
                {
                    Finished = false,
                    CanStartNextLabirint = false,
                    Message = "There is no unfinished labirint. Start next labirint or finish and start game."
                };
            }

            curLabirint.FinishTime = DateTime.UtcNow;
            await _labirintRepository.UpdateAsync(curLabirint);

            bool finishedInTime = lastGame.TimeFreezeUsed || (curLabirint.FinishTime.Value - curLabirint.StartTime.Value).TotalSeconds < curLabirint.TimeToFinishSeconds;

            return new FinishLabirintResponse()
            {
                Finished = true,
                CanStartNextLabirint = finishedInTime && nextLabirint != null,
                LabirintBtcAmount = curLabirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Btc).Sum(x => x.Amount),
                LabirintEthAmount = curLabirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Eth).Sum(x => x.Amount),
                LabirintTonAmount = curLabirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Ton).Sum(x => x.Amount),
                LabirintCryptoKeyFragments = curLabirint.Energies.Where(x => x.Found).Sum(x => x.Amount),
                LabirintEnergyAmount = curLabirint.CryptoKeyFragments.Where(x => x.Found).Sum(x => x.Amount),
            };
        }

        public async Task<FinishGameResponse> FinishGameAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new FinishGameResponse()
                {
                    Finished = true,
                    Message = "You have finished last game already."
                };
            }

            var curLabirint = lastGame.Labirints
                .OrderBy(x => x.LabirintOrderId)
                .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);
            var incompletedLabirint = lastGame.Labirints
                .OrderBy(x => x.LabirintOrderId)
                .FirstOrDefault(x => !x.StartTime.HasValue);
            var lastFinishedLabirint = lastGame.Labirints
                .OrderByDescending(x => x.LabirintOrderId)
                .FirstOrDefault(x => x.StartTime.HasValue && x.FinishTime.HasValue);

            if (curLabirint != null)
            {
                return new FinishGameResponse()
                {
                    Finished = false,
                    Message = "Finish current labirint first."
                };
            }

            var finishedInTime = lastGame.TimeFreezeUsed || (lastFinishedLabirint.FinishTime.Value - lastFinishedLabirint.StartTime.Value).TotalSeconds < lastFinishedLabirint.TimeToFinishSeconds;
            
            if (finishedInTime && incompletedLabirint != null)
            {
                return new FinishGameResponse()
                {
                    Finished = false,
                    Message = "Finish all labirints first."
                };
            }

            CalculateGameResults(lastGame);
            lastGame.FinishTime = DateTime.UtcNow;
            await _gameRepository.UpdateAsync(lastGame);

            var userItems = await GetOrAddUserItems(email, new ItemType[] { ItemType.BtcBlock, ItemType.EthBlock, ItemType.TonBlock, ItemType.Energy, ItemType.CryptoKeyFragment });

            var btcBlock = userItems.FirstOrDefault(x => x.Item.Type == Common.ItemType.BtcBlock);
            var ethBlock = userItems.FirstOrDefault(x => x.Item.Type == Common.ItemType.EthBlock);
            var tonBlock = userItems.FirstOrDefault(x => x.Item.Type == Common.ItemType.TonBlock);
            var energy = userItems.FirstOrDefault(x => x.Item.Type == Common.ItemType.Energy);
            var cryptoKeyFragments = userItems.FirstOrDefault(x => x.Item.Type == Common.ItemType.CryptoKeyFragment);

            btcBlock.ItemAmount += lastGame.BtcBlocksCollected;
            ethBlock.ItemAmount += lastGame.EthBlocksCollected;
            tonBlock.ItemAmount += lastGame.TonBlocksCollected;
            energy.ItemAmount += lastGame.EnergyCollected;
            cryptoKeyFragments.ItemAmount += lastGame.CryptoKeyFragmentsCollected;

            await _userItemRepository.UpdateRangeAsync(new UserItem[] { btcBlock, ethBlock, tonBlock, energy, cryptoKeyFragments });

            return new FinishGameResponse()
            {
                Finished = true,
                TotalBtcAmount = lastGame.BtcBlocksCollected,
                TotalEthAmount = lastGame.EthBlocksCollected,
                TotalTonAmount = lastGame.TonBlocksCollected,
                TotalCryptoKeyFragments = lastGame.CryptoKeyFragmentsCollected,
                TotalEnergyAmount = lastGame.EnergyCollected,
            };
        }

        public async Task<OpenStorageResponse> OpenStorageAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if (!lastGame.TimeFreezeUsed && (DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "Labirint timeout."
                };
            }

            if (!curLabirint.HasCryptoStorage)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "This labirint has no cryptostorage. Suspicious activity."
                };
            }

            if (curLabirint.CryptoStorageOpened)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "Cryptostorage has been already opened. Suspicious activity."
                };
            }

            var cryptoKeyItem = await _userItemRepository.GetUserItem(email, Common.ItemType.CryptoKey);
            if (cryptoKeyItem == null || cryptoKeyItem.ItemAmount < 1)
            {
                return new OpenStorageResponse()
                {
                    Opened = false,
                    Message = "You have no crypto keys."
                };
            }

            cryptoKeyItem.ItemAmount -= 1;
            await _userItemRepository.UpdateAsync(cryptoKeyItem);

            curLabirint.CryptoStorageOpened = true;
            await _labirintRepository.UpdateAsync(curLabirint);

            lastGame.CryptoKeyUsed = true;
            await _gameRepository.UpdateAsync(lastGame);

            return new OpenStorageResponse()
            {
                Opened = true,
                Message = string.Empty
            };
        }

        public async Task<CollectCryptoBlockResponse> CollectCryptoBlockAsync(string email, Guid id)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if (!lastGame.TimeFreezeUsed && (DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "Labirint timeout."
                };
            }

            var cryptoBlock = curLabirint.CryptoBlocks.FirstOrDefault(x => x.Id == id);
            if (cryptoBlock == null)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "Invalid crypto block Id. Suspicious activity."
                };
            }

            if (cryptoBlock.Found)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "Crypto block has been already found. Suspicious activity."
                };
            }

            if (cryptoBlock.Storage && !curLabirint.CryptoStorageOpened)
            {
                return new CollectCryptoBlockResponse()
                {
                    Collected = false,
                    Message = "Can't collect crypto block, storage hasn't opened yet. Suspicious activity."
                };
            }

            cryptoBlock.Found = true;
            await _labirintCryptoBlockRepository.UpdateAsync(cryptoBlock);

            return new CollectCryptoBlockResponse()
            {
                Collected = true,
                Message = string.Empty,
                CollectedType = cryptoBlock.Type,
                CollectedAmount = cryptoBlock.Amount
            };
        }

        public async Task<CollectEnergyResponse> CollectEnergyAsync(string email, Guid id)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if (!lastGame.TimeFreezeUsed && (DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "Labirint timeout."
                };
            }

            var energy = curLabirint.Energies.FirstOrDefault(x => x.Id == id);
            if (energy == null)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "Invalid energy Id. Suspicious activity."
                };
            }

            if (energy.Found)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "Energy has been already found. Suspicious activity."
                };
            }

            if (energy.Storage && !curLabirint.CryptoStorageOpened)
            {
                return new CollectEnergyResponse()
                {
                    Collected = false,
                    Message = "Can't collect crypto block, storage hasn't opened yet. Suspicious activity."
                };
            }

            energy.Found = true;
            await _labirintEnergyRepository.UpdateAsync(energy);

            return new CollectEnergyResponse()
            {
                Collected = true,
                Message = string.Empty,
                CollectedAmount = energy.Amount
            };
        }

        public async Task<CollectCryptoKeyFragmentResponse> CollectCryptoKeyFragmentAsync(string email, Guid id)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if (!lastGame.TimeFreezeUsed && (DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "Labirint timeout."
                };
            }

            var cryptoKeyFragment = curLabirint.CryptoKeyFragments.FirstOrDefault(x => x.Id == id);
            if (cryptoKeyFragment == null)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "Invalid crypto key fragment Id. Suspicious activity."
                };
            }

            if (cryptoKeyFragment.Found)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "Crypto key fragment has been already found. Suspicious activity."
                };
            }

            if (cryptoKeyFragment.Storage && !curLabirint.CryptoStorageOpened)
            {
                return new CollectCryptoKeyFragmentResponse()
                {
                    Collected = false,
                    Message = "Can't collect crypto block, storage hasn't opened yet. Suspicious activity."
                };
            }

            cryptoKeyFragment.Found = true;
            await _labirintLabirintCryptoKeyFragmentRepository.UpdateAsync(cryptoKeyFragment);

            return new CollectCryptoKeyFragmentResponse()
            {
                Collected = true,
                Message = string.Empty,
                CollectedAmount = cryptoKeyFragment.Amount
            };
        }

        public async Task<UseSpeedRocketBoosterResponse> UseSpeedRocketBoosterAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new UseSpeedRocketBoosterResponse()
                {
                    Used = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new UseSpeedRocketBoosterResponse()
                {
                    Used = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if (curLabirint.SpeedRocketUsed)
            {
                return new UseSpeedRocketBoosterResponse()
                {
                    Used = false,
                    Message = "Speed rocket booster has been already used in this labirint."
                };
            }

            if (!lastGame.TimeFreezeUsed && (DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new UseSpeedRocketBoosterResponse()
                {
                    Used = false,
                    Message = "Labirint timeout."
                };
            }

            var speedRocketUserItem = await _userItemRepository.GetUserItem(email, Common.ItemType.SpeedRocketBooster);
            if (speedRocketUserItem == null || speedRocketUserItem.ItemAmount < 1)
            {
                return new UseSpeedRocketBoosterResponse()
                {
                    Used = false,
                    Message = "You have no speed rocket boosters."
                };
            }

            speedRocketUserItem.ItemAmount -= 1;
            await _userItemRepository.UpdateAsync(speedRocketUserItem);

            curLabirint.SpeedRocketUsed = true;
            await _labirintRepository.UpdateAsync(curLabirint);

            return new UseSpeedRocketBoosterResponse()
            {
                Used = true,
                Message = string.Empty
            };
        }

        public async Task<UseFreezeTimeBoosterResponse> UseFreezeTimeBoosterAsync(string email)
        {
            var lastGame = await _gameRepository.GetLastGame(email);

            if (lastGame == null || lastGame.FinishTime.HasValue)
            {
                return new UseFreezeTimeBoosterResponse()
                {
                    Used = false,
                    Message = "You have finished last game already. Start new game."
                };
            }

            if (lastGame.TimeFreezeUsed)
            {
                return new UseFreezeTimeBoosterResponse()
                {
                    Used = false,
                    Message = "Time freeze booster has been already used."
                };
            }

            var curLabirint = lastGame.Labirints
                    .OrderBy(x => x.LabirintOrderId)
                    .FirstOrDefault(x => x.StartTime.HasValue && !x.FinishTime.HasValue);

            if (curLabirint == null)
            {
                return new UseFreezeTimeBoosterResponse()
                {
                    Used = false,
                    Message = "You have finished last game already or haven't start game yet."
                };
            }

            if ((DateTime.UtcNow - curLabirint.StartTime.Value).TotalSeconds > curLabirint.TimeToFinishSeconds)
            {
                return new UseFreezeTimeBoosterResponse()
                {
                    Used = false,
                    Message = "Labirint timeout."
                };
            }

            var timeFreezeUserItem = await _userItemRepository.GetUserItem(email, Common.ItemType.FreezeTimeBooster);
            if (timeFreezeUserItem == null || timeFreezeUserItem.ItemAmount < 1)
            {
                return new UseFreezeTimeBoosterResponse()
                {
                    Used = false,
                    Message = "You have no freeze time boosters."
                };
            }

            timeFreezeUserItem.ItemAmount -= 1;
            await _userItemRepository.UpdateAsync(timeFreezeUserItem);

            lastGame.TimeFreezeUsed = true;
            await _gameRepository.UpdateAsync(lastGame);

            return new UseFreezeTimeBoosterResponse()
            {
                Used = true,
                Message = string.Empty
            };
        }

        private IEnumerable<LabirintCryptoBlock> GenerateCryptoBlocks(Labirint labirint, Common.CryptoType type, int count, float probability, bool storage)
        {
            List<LabirintCryptoBlock> blocks = new List<LabirintCryptoBlock>(count);

            for (int i = 0; i < count; ++i)
            {
                var randomValue = _random.NextSingle();

                if (randomValue < probability)
                {
                    blocks.Add(new LabirintCryptoBlock()
                    {
                        Type = type,
                        Amount = GenerateCryptoBlockAmount(type),
                        Found = false,
                        Storage = storage,
                        LabirintId = labirint.Id,
                        Labirint = labirint
                    });
                }
            }

            return blocks;
        }

        private IEnumerable<LabirintCryptoKeyFragment> GenerateCryptoKeyFragments(Labirint labirint, int count, float probability, bool storage)
        {
            List<LabirintCryptoKeyFragment> fragments = new List<LabirintCryptoKeyFragment>(count);

            for (int i = 0; i < count; ++i)
            {
                var randomValue = _random.NextSingle();

                if (randomValue < probability)
                {
                    fragments.Add(new LabirintCryptoKeyFragment()
                    {
                        Found = false,
                        Amount = GenerateAmount(_gameConfiguration.CryptoKeyFragmentMinAmount, _gameConfiguration.CryptoKeyFragmentMaxAmount),
                        LabirintId = labirint.Id,
                        Labirint = labirint
                    });
                }
            }

            return fragments;
        }

        private IEnumerable<LabirintEnergy> GenerateEnergies(Labirint labirint, int count, float probability, bool storage)
        {
            List<LabirintEnergy> energies = new List<LabirintEnergy>(count);

            for (int i = 0; i < count; ++i)
            {
                var randomValue = _random.NextSingle();

                if (randomValue < probability)
                {
                    energies.Add(new LabirintEnergy()
                    {
                        Found = false,
                        Amount = GenerateAmount(_gameConfiguration.EnergyMinAmount, _gameConfiguration.EnergyMaxAmount),
                        LabirintId = labirint.Id,
                        Labirint = labirint
                    });
                }
            }

            return energies;
        }

        private int GenerateCryptoBlockAmount(Common.CryptoType type)
        {
            var amount = 0;
            if (type == Common.CryptoType.Btc)
            {
                amount = GenerateAmount(_gameConfiguration.BtcBlockMinAmount, _gameConfiguration.BtcBlockMaxAmount);
            }
            else if (type == Common.CryptoType.Eth)
            {
                amount = GenerateAmount(_gameConfiguration.EthBlockMinAmount, _gameConfiguration.EthBlockMaxAmount);
            }
            else if (type == Common.CryptoType.Ton)
            {
                amount = GenerateAmount(_gameConfiguration.TonBlockMinAmount, _gameConfiguration.TonBlockMaxAmount);
            }

            return amount;
        }

        private int GenerateAmount(int min, int max)
        {
            return _random.Next(min, max + 1);
        }

        private UserItem CreateUserItem(User user, Item item)
        {
            return new UserItem()
            {
                ItemAmount = 0,
                UserId = user.Id,
                User = user,
                ItemId = item.Id,
                Item = item
            };
        }

        private async Task<IEnumerable<UserItem>> GetOrAddUserItems(string email, ItemType[] itemTypes)
        {
            IEnumerable<Item> items = null;
            List<UserItem> itemsToAdd = new List<UserItem>(itemTypes.Length);

            var user = await _userRepository.GetUserByEmailAsync(email);

            foreach (var itemType in itemTypes)
            {
                var userItem = user.Items.FirstOrDefault(x => x.Item.Type == itemType);
                
                if (userItem == null)
                {
                    if (items == null)
                    {
                        items = await _itemRepository.GetAsync();
                    }

                    userItem = CreateUserItem(user, items.FirstOrDefault(x => x.Type == itemType));
                    itemsToAdd.Add(userItem);
                }
            }

            if (itemsToAdd.Any())
            {
                await _userItemRepository.AddRangeAsync(itemsToAdd);
            }

            return await _userItemRepository.GetUserItems(email);
        }

        private void CalculateGameResults(Game lastGame)
        {
            int totalBtcAmount = 0;
            int totalEthAmount = 0;
            int totalTonAmount = 0;
            int totalEnergyAmount = 0;
            int totalCryptoKeyFragments = 0;

            foreach (var labirint in lastGame.Labirints)
            {
                if (labirint.StartTime.HasValue)
                {
                    var finishedInTime = lastGame.TimeFreezeUsed ||  (labirint.FinishTime.Value - labirint.StartTime.Value).TotalSeconds < labirint.TimeToFinishSeconds;
                    
                    if (finishedInTime)
                    {
                        totalBtcAmount += labirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Btc).Sum(x => x.Amount);
                        totalEthAmount += labirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Eth).Sum(x => x.Amount);
                        totalTonAmount += labirint.CryptoBlocks.Where(x => x.Found && x.Type == Common.CryptoType.Ton).Sum(x => x.Amount);
                        totalEnergyAmount += labirint.Energies.Where(x => x.Found).Sum(x => x.Amount);
                        totalCryptoKeyFragments += labirint.CryptoKeyFragments.Where(x => x.Found).Sum(x => x.Amount);
                    }
                }
            }

            lastGame.BtcBlocksCollected = totalBtcAmount;
            lastGame.EthBlocksCollected = totalEthAmount;
            lastGame.TonBlocksCollected = totalTonAmount;
            lastGame.EnergyCollected = totalEnergyAmount;
            lastGame.CryptoKeyFragmentsCollected = totalCryptoKeyFragments;
        }
    }
}
