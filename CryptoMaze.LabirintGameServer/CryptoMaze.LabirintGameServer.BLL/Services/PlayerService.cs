using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.BLL.Utils;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Repositories;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IUserItemRepository _userItemRepository;
        private readonly IGenericRepository<Item, int> _itemRepository;
        private readonly GameConfiguration _gameConfiguration;

        public PlayerService(IUserRepository userRepository, IGameRepository gameRepository,
            IUserItemRepository userItemRepository,
            IGenericRepository<Item, int> itemRepository, GameConfiguration gameConfiguration)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _userItemRepository = userItemRepository;
            _itemRepository = itemRepository;
            _gameConfiguration = gameConfiguration;
        }

        public async Task<PlayerDataResponse> GetDataAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                user = new User()
                {
                    Email = email,
                    Name = Base64Ext.EncodeNoPadding(Guid.NewGuid().ToByteArray()),
                    NameChanged = false
                };

                await _userRepository.AddAsync(user);

                var items = await _itemRepository.GetAsync();
                var userItems = items
                    .Select(x => new UserItem()
                    {
                        ItemAmount = 0,
                        ItemId = x.Id,
                        Item = x,
                        UserId = user.Id,
                        User = user
                    })
                    .ToList();

                await _userItemRepository.AddRangeAsync(userItems);

                user.Items = userItems;
            }

            var lastGame = await _gameRepository.GetLastGame(email);

            return new PlayerDataResponse()
            {
                Email = user.Email,
                Name = user.Name,
                NameChanged = user.NameChanged,
                EnergyStartGameNeeded = _gameConfiguration.EnergyStartGameNeeded,
                LastGameFinished = lastGame == null || lastGame.FinishTime.HasValue,
                Items = user.Items.Select(x => new UserItemModel()
                {
                    Type = x.Item.Type,
                    Amount = x.ItemAmount
                })
                .ToList()
            };
        }

        public async Task<SetNameResponse> SetNameAsync(string email, string name)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new SetNameResponse()
                {
                    Updated = false,
                    Name = string.Empty,
                };
            }

            if (user.NameChanged)
            {
                return new SetNameResponse()
                {
                    Updated = false,
                    Name = user.Name,
                };
            }

            user.Name = name;
            user.NameChanged = true;

            await _userRepository.UpdateAsync(user);

            return new SetNameResponse()
            {
                Updated = true,
                Name = user.Name,
            };
        }

        public async Task<AddEnergyResponse> AddEnergyAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new AddEnergyResponse()
                {
                    EnergyAddedCount = 0,
                    CurrentEnergyAmount = 0
                };
            }

            var energyItem = user.Items.FirstOrDefault(x => x.Item.Type == Common.ItemType.Energy);
            if (energyItem == null)
            {
                var items = await _itemRepository.GetAsync();
                var item = items.FirstOrDefault(x => x.Type == Common.ItemType.Energy);

                energyItem = new UserItem()
                {
                    ItemAmount = 0,
                    UserId = user.Id,
                    User = user,
                    ItemId = item.Id,
                    Item = item
                };

                await _userItemRepository.AddAsync(energyItem);
            }

            energyItem.ItemAmount += _gameConfiguration.EnergyStartGameNeeded;
            await _userItemRepository.UpdateAsync(energyItem);

            return new AddEnergyResponse()
            {
                EnergyAddedCount = _gameConfiguration.EnergyStartGameNeeded,
                CurrentEnergyAmount = energyItem.ItemAmount
            };
        }
    }
}
