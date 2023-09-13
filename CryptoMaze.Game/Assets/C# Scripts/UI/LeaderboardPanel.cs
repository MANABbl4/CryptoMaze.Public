using CryptoMaze.Common;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    public Action<CryptoType> OnTypeChanged;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _btcButton;

    [SerializeField]
    private Button _ethButton;

    [SerializeField]
    private Button _tonButton;

    [SerializeField]
    private Image _btcImage;

    [SerializeField]
    private Image _ethImage;

    [SerializeField]
    private Image _tonImage;

    public CryptoType CurrentType { get; private set; } = CryptoType.Btc;

    private void Start()
    {
        _btcButton.onClick.AddListener(() => { ChangeType(CryptoType.Btc); });
        _ethButton.onClick.AddListener(() => { ChangeType(CryptoType.Eth); });
        _tonButton.onClick.AddListener(() => { ChangeType(CryptoType.Ton); });

        UpdateButtons();
    }

    private void ChangeType(CryptoType type)
    {
        if (CurrentType != type)
        {
            _audioSource.Play();

            CurrentType = type;

            UpdateButtons();

            OnTypeChanged?.Invoke(type);
        }
    }

    private void UpdateButtons()
    {
        _btcButton.gameObject.SetActive(CurrentType != CryptoType.Btc);
        _ethButton.gameObject.SetActive(CurrentType != CryptoType.Eth);
        _tonButton.gameObject.SetActive(CurrentType != CryptoType.Ton);

        _btcImage.gameObject.SetActive(CurrentType == CryptoType.Btc);
        _ethImage.gameObject.SetActive(CurrentType == CryptoType.Eth);
        _tonImage.gameObject.SetActive(CurrentType == CryptoType.Ton);
    }
}
