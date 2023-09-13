using System;
using System.Collections.Generic;
using System.Linq;
using CryptoMaze.Client;
using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.Common;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private LeaderboardScrollView _scrollView;

    [SerializeField]
    private LeaderboardPanel _leaderboardPanel;

    [SerializeField]
    private LeaderboardRewardDialog _rewardDialog;

    [SerializeField]
    private SeasonTimer _seasonTimer;

    [SerializeField]
    private Text _seasonNumber;


    private CryptoMazeClient _cryptoMazeClient = null;
    private Dictionary<CryptoType, List<GameResult>> _topResults = null;
    private Dictionary<CryptoType, GameResult> _playerResults = null;
    private int _topCount = 0;

    private void Start()
    {
        _topResults = new Dictionary<CryptoType, List<GameResult>>
        {
            { CryptoType.Btc, new List<GameResult>() },
            { CryptoType.Eth, new List<GameResult>() },
            { CryptoType.Ton, new List<GameResult>() }
        };

        _playerResults = new Dictionary<CryptoType, GameResult>
        {
            { CryptoType.Btc, null },
            { CryptoType.Eth, null },
            { CryptoType.Ton, null }
        };

        _rewardDialog.gameObject.SetActive(false);

        _leaderboardPanel.OnTypeChanged += OnUpdateLeaderboard;

        _scrollView.OnRankRewardClicked += OnShowRewardDialog;

        _seasonTimer.OnSeasonFinished += OnGetLeaderboard;

        _cryptoMazeClient = SceneHelper.CreateAndValidateClient();

        OnGetLeaderboard();
    }

    private void OnUpdateLeaderboard(CryptoType cryptoType)
    {
        _scrollView.SetData(_topResults[cryptoType], _playerResults[cryptoType], _topCount);
    }

    private void OnShowRewardDialog(int rank)
    {
        var result = _topResults[_leaderboardPanel.CurrentType].FirstOrDefault(x => x.rank == rank);
        
        if (result == null)
        {
            if (_playerResults[_leaderboardPanel.CurrentType].rank == rank)
            {
                result = _playerResults[_leaderboardPanel.CurrentType];
            }
        }

        if (result == null)
        {
            Debug.LogError($"Failed find reward for rank {rank}");
        }

        _rewardDialog.SetRewardData(result.reward, result.rank);
        _rewardDialog.gameObject.SetActive(true);
    }

    private void OnGetLeaderboard()
    {
        _cryptoMazeClient.Game
            .GetLeaderboardData()
            .Then(leaderboardData =>
            {
                try
                {
                    _seasonNumber.text = leaderboardData.leaderboard.seasonNumber.ToString();
                    _seasonTimer.SetTimeLeft(leaderboardData.leaderboard.timeLeft);

                    _topCount = leaderboardData.leaderboard.topCount;

                    _topResults[CryptoType.Btc].AddRange(leaderboardData.leaderboard.btcTopResults);
                    _topResults[CryptoType.Eth].AddRange(leaderboardData.leaderboard.ethTopResults);
                    _topResults[CryptoType.Ton].AddRange(leaderboardData.leaderboard.tonTopResults);

                    _playerResults[CryptoType.Btc] = leaderboardData.playerBtcResult;
                    _playerResults[CryptoType.Eth] = leaderboardData.playerEthResult;
                    _playerResults[CryptoType.Ton] = leaderboardData.playerTonResult;

                    OnUpdateLeaderboard(_leaderboardPanel.CurrentType);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());

                PlayerPrefs.SetString("accessToken", string.Empty);
                PlayerPrefs.SetString("refreshToken", string.Empty);

                SceneManager.LoadScene(0);
            });
    }
}