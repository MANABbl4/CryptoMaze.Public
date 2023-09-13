using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LeaderboardScrollItem : MonoBehaviour, IPointerClickHandler
{
    public Action<int> OnRewardImageClicked;

    [SerializeField]
    private Image _backgroundImage;

    [SerializeField]
    private Image _rewardImage;

    [SerializeField]
    private Image _rankFrameGoldImage;

    [SerializeField]
    private Image _rankFrameSilverImage;

    [SerializeField]
    private Image _rankFrameBronzeImage;

    [SerializeField]
    private Image _rankFrameImage;

    [SerializeField]
    private Sprite _firstPlace;

    [SerializeField]
    private Sprite _secondPlace;

    [SerializeField]
    private Sprite _thirdPlace;

    [SerializeField]
    private Sprite _playerPlace;

    [SerializeField]
    private Sprite _otherPlace;

    [SerializeField]
    private Sprite _rewardFirstPlace;

    [SerializeField]
    private Sprite _rewardSecondPlace;

    [SerializeField]
    private Sprite _rewardThirdPlace;

    [SerializeField]
    private Sprite _rewardTopPlace;

    [SerializeField]
    private Sprite _rewardOtherPlace;


    [SerializeField]
    private Text _nameText;

    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _rankText;

    private Dictionary<int, Sprite> _backgroundRankSprites = null;
    private Dictionary<int, Sprite> _rewardRankSprites = null;
    private int _rank = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rewardImage.rectTransform, eventData.position, eventData.pressEventCamera, out var localPoint))
        {
            if (_rewardImage.rectTransform.rect.Contains(localPoint))
            {
                OnRewardImageClicked?.Invoke(_rank);
            }
        }
    }

    public void SetData(string name, int score, int rank, bool topPlace, bool playerData)
    {
        _rank = rank;

        _nameText.text = name;
        _scoreText.text = score.ToString();
        _rankText.text = rank.ToString();

        _rankFrameGoldImage.gameObject.SetActive(rank == 1);
        _rankFrameSilverImage.gameObject.SetActive(rank == 2);
        _rankFrameBronzeImage.gameObject.SetActive(rank == 3);
        _rankFrameImage.gameObject.SetActive(rank > 3);

        _rewardImage.gameObject.SetActive(true);

        if (playerData)
        {
            _backgroundImage.sprite = _playerPlace;
        }
        else if (_backgroundRankSprites.ContainsKey(rank))
        {
            _backgroundImage.sprite = _backgroundRankSprites[rank];
        }
        else
        {
            _backgroundImage.sprite = _otherPlace;
        }

        if (_rewardRankSprites.ContainsKey(rank))
        {
            _rewardImage.sprite = _rewardRankSprites[rank];
        }
        else if (topPlace)
        {
            _rewardImage.sprite = _rewardTopPlace;
        }
        else if (score > 0)
        {
            _rewardImage.sprite = _rewardOtherPlace;
        }
        else
        {
            _rewardImage.sprite = null;
            _rewardImage.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _backgroundRankSprites = new Dictionary<int, Sprite>
        {
            { 1, _firstPlace },
            { 2, _secondPlace },
            { 3, _thirdPlace }
        };

        _rewardRankSprites = new Dictionary<int, Sprite>
        {
            { 1, _rewardFirstPlace },
            { 2, _rewardSecondPlace },
            { 3, _rewardThirdPlace }
        };
    }
}
