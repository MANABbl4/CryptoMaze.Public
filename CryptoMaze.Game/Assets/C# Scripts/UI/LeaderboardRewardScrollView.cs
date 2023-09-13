using CryptoMaze.ClientServer.Game.DataContainers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardRewardScrollView : MonoBehaviour
{
    [SerializeField]
    private LeaderboardRewardScrollItem _scollItemPrefab;

    [SerializeField]
    private List<ItemSprite> _itemTypeSprites;

    private List<LeaderboardRewardScrollItem> _scrollItems = new List<LeaderboardRewardScrollItem>();
    private Queue<LeaderboardRewardScrollItem> _scrollItemPool = new Queue<LeaderboardRewardScrollItem>();

    public void SetData(UserItemModel[] reward)
    {
        foreach (var scrollItem in _scrollItems)
        {
            scrollItem.gameObject.SetActive(false);
            _scrollItemPool.Enqueue(scrollItem);
        }

        _scrollItems.Clear();

        var transforms = gameObject.GetComponentsInChildren<RectTransform>();
        var scrollViewContent = transforms.FirstOrDefault(x => x.name == "Content");

        for (int i = 0; i < reward.Length; ++i)
        {
            var item = reward[i];
            var scrollItem = CreateItem(_scollItemPrefab, scrollViewContent, item);
            _scrollItems.Add(scrollItem);
        }
    }

    private LeaderboardRewardScrollItem CreateItem(LeaderboardRewardScrollItem scrollItemPrefab, RectTransform scrollViewContent,
        UserItemModel item)
    {
        var scrollItem = GetScrollItem(scrollItemPrefab, scrollViewContent);
        scrollItem.SetData(item.amount, _itemTypeSprites.First(x => x.ItemType == item.Type).Sprite);

        return scrollItem;
    }

    private LeaderboardRewardScrollItem GetScrollItem(LeaderboardRewardScrollItem scrollItemPrefab, RectTransform scrollViewContent)
    {
        if (_scrollItemPool.Count > 0)
        {
            var scrollItem = _scrollItemPool.Dequeue();
            scrollItem.gameObject.SetActive(true);

            return scrollItem;
        }

        return Instantiate(scrollItemPrefab, scrollViewContent);
    }
}
