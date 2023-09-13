using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.LabirintGameServer.DAL.Entities;
using CryptoMaze.LabirintGameServer.DAL.Repositories;

namespace CryptoMaze.LabirintGameServer.BLL.Services
{
    public class ShopService : IShopService
    {
        private readonly IShopProposalRepository _shopProposalRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserItemRepository _userItemRepository;

        public ShopService(IShopProposalRepository shopProposalRepository, IUserRepository userRepository,
            IUserItemRepository userItemRepository)
        {
            _shopProposalRepository = shopProposalRepository;
            _userRepository = userRepository;
            _userItemRepository = userItemRepository;
        }

        public async Task<ShopDataResponse> GetDataAsync()
        {
            var results = await _shopProposalRepository.GetAsync();

            return new ShopDataResponse()
            {
                ShopProposals = results.Select(x => new ShopProposalModel
                {
                    ProposalId = x.Id,
                    BuyType = x.BuyItem.Type,
                    BuyAmount = x.BuyItemAmount,
                    BuyShopType = x.BuyShopItemType,
                    SellType = x.SellItem.Type,
                    SellAmount = x.SellItemAmount,
                    SellShopType = x.SellShopItemType,
                    Order = x.Order
                })
                .OrderBy(x => x.Order)
                .ToList()
            };
        }

        public async Task<BuyResponse> BuyAsync(string email, int proposalId)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null)
            {
                return new BuyResponse() { Bought = false };
            }

            var proposal = await _shopProposalRepository.GetByIdAsync(proposalId);

            if (proposal == null)
            {
                return new BuyResponse() { Bought = false };
            }

            var userSellItem = user.Items.FirstOrDefault(x => x.Item.Type == proposal.SellItem.Type);

            if (userSellItem == null || userSellItem.ItemAmount < proposal.SellItemAmount)
            {
                return new BuyResponse() { Bought = false };
            }

            var userBuyItem = user.Items.FirstOrDefault(x => x.Item.Type == proposal.BuyItem.Type);

            if (userBuyItem == null)
            {
                userBuyItem = new UserItem()
                {
                    ItemAmount = 0,
                    ItemId = proposal.BuyItem.Id,
                    Item = proposal.BuyItem,
                    UserId = user.Id,
                    User = user
                };

                await _userItemRepository.AddAsync(userBuyItem);
            }

            userBuyItem.ItemAmount += proposal.BuyItemAmount;
            userSellItem.ItemAmount -= proposal.SellItemAmount;

            await _userItemRepository.UpdateRangeAsync(new UserItem[] { userBuyItem, userSellItem });

            return new BuyResponse()
            {
                Bought = true,
                BoughtItemUpdatedValue = new UserItemModel() { Amount = userBuyItem.ItemAmount, Type = userBuyItem.Item.Type },
                SoldItemUpdatedValue = new UserItemModel() { Amount = userSellItem.ItemAmount, Type = userSellItem.Item.Type }
            };
        }
    }
}
