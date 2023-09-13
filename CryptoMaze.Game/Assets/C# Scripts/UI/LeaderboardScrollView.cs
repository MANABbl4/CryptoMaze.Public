using CryptoMaze.ClientServer.Game.DataContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Networking.UnityWebRequest;

public class LeaderboardScrollView : MonoBehaviour
{
    public Action<int> OnRankRewardClicked;

    [SerializeField]
    private LeaderboardScrollItem _scollItemPrefab;

    private List<LeaderboardScrollItem> _scrollItems = new List<LeaderboardScrollItem>();
    private Queue<LeaderboardScrollItem> _scrollItemPool = new Queue<LeaderboardScrollItem>();

    public void SetData(List<GameResult> topResults, GameResult playerResult, int topCount)
    {
        foreach (var scrollItem in _scrollItems)
        {
            scrollItem.OnRewardImageClicked -= OnReward;
            scrollItem.gameObject.SetActive(false);
            _scrollItemPool.Enqueue(scrollItem);
        }

        _scrollItems.Clear();

        var transforms = gameObject.GetComponentsInChildren<RectTransform>();
        var scrollViewContent = transforms.FirstOrDefault(x => x.name == "Content");
        bool hasPlayerData = false;

        for (int i = 0; i < topResults.Count; ++i)
        {
            var result = topResults[i];

            if (!hasPlayerData)
            {
                hasPlayerData = result.rank == playerResult.rank;
            }

            var scrollItem = CreateItem(_scollItemPrefab, scrollViewContent, result, result.rank <= topCount, result.rank == playerResult.rank);
            _scrollItems.Add(scrollItem);
        }

        if (!hasPlayerData)
        {
            var scrollItem = CreateItem(_scollItemPrefab, scrollViewContent, playerResult, playerResult.rank <= topCount, true);
            _scrollItems.Add(scrollItem);
        }

        foreach (var scrollItem in _scrollItems)
        {
            scrollItem.OnRewardImageClicked += OnReward;
        }
    }

    private LeaderboardScrollItem CreateItem(LeaderboardScrollItem scrollItemPrefab, RectTransform scrollViewContent,
        GameResult result, bool topPlace, bool playerData)
    {
        var scrollItem = GetScrollItem(scrollItemPrefab, scrollViewContent);
        scrollItem.SetData(result.userName, result.result, result.rank, topPlace, playerData);

        return scrollItem;
    }

    private LeaderboardScrollItem GetScrollItem(LeaderboardScrollItem scrollItemPrefab, RectTransform scrollViewContent)
    {
        if (_scrollItemPool.Count > 0)
        {
            var scrollItem = _scrollItemPool.Dequeue();
            scrollItem.gameObject.SetActive(true);

            return scrollItem;
        }

        return Instantiate(scrollItemPrefab, scrollViewContent);
    }

    private void OnReward(int rank)
    {
        OnRankRewardClicked?.Invoke(rank);
    }
}