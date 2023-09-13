using CryptoMaze.ClientServer.Game.DataContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopScrollView : MonoBehaviour
{
    public Action<int> OnBuyItem;

    [SerializeField]
    private ShopScrollItem _scollItemPrefab;

    [SerializeField]
    private List<ShopItemSprite> _itemSprites;

    private List<ShopScrollItem> _scrollItems = new List<ShopScrollItem>();
    private List<ShopProposalModel> _proposals = null;

    public void SetProposals(List<ShopProposalModel> proposals)
    {
        if (_proposals != null)
        {
            foreach (var scrollItem in _scrollItems)
            {
                scrollItem.OnBuy -= OnBuyItem;
                Destroy(scrollItem.gameObject);
            }

            _scrollItems.Clear();
        }

        _proposals = proposals;

        var transforms = gameObject.GetComponentsInChildren<RectTransform>();
        var scrollViewContent = transforms.FirstOrDefault(x => x.name == "Content");

        for (int i = 0; i < _proposals.Count; ++i)
        {
            var proposal = _proposals[i];
            var scrollItem = CreateItem(_scollItemPrefab, scrollViewContent, proposal);
            _scrollItems.Add(scrollItem);
        }
    }

    private ShopScrollItem CreateItem(ShopScrollItem scrollItemPrefab, RectTransform scrollViewContent, ShopProposalModel proposal)
    {
        var scrollItem = Instantiate(scrollItemPrefab, scrollViewContent);
        scrollItem.SetProposal(proposal,
            _itemSprites.First(x => x.ItemType == proposal.BuyShopType).Sprite,
            _itemSprites.First(x => x.ItemType == proposal.SellShopType).Sprite);

        scrollItem.OnBuy += OnBuy;

        return scrollItem;
    }

    private void OnBuy(int proposalId)
    {
        OnBuyItem?.Invoke(proposalId);
    }
}
