using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public Action OnFreezeTimeClicked;
    public Action OnSpeedRocketClicked;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _freezeTimeButton;

    [SerializeField]
    private Button _speedRocketButton;

    [SerializeField]
    private Text _timeFreezeBoosters;

    [SerializeField]
    private Text _speedRocketBoosters;

    [SerializeField]
    private Text _cryptoKeys;

    [SerializeField]
    private Text _cryptoKeyFragments;

    public int TimeFreezeBoostersAmount { get; private set; }
    public int SpeedRocketBoostersAmount { get; private set; }
    public int CryptoKeysAmount { get; private set; }
    public int CryptoKeyFragments { get; private set; }


    public void AddCryptoKeys(int amount)
    {
        CryptoKeysAmount += amount;
        _cryptoKeys.text = CryptoKeysAmount.ToString();
    }

    public void AddCryptoKeyFragments(int amount)
    {
        CryptoKeyFragments += amount;
        _cryptoKeyFragments.text = CryptoKeyFragments.ToString();
    }

    public void RemoveTimeFreezeBoosters(int amount)
    {
        TimeFreezeBoostersAmount -= amount;
        _timeFreezeBoosters.text = TimeFreezeBoostersAmount.ToString();
    }

    public void RemoveSpeedRocketBoosters(int amount)
    {
        SpeedRocketBoostersAmount -= amount;
        _speedRocketBoosters.text = SpeedRocketBoostersAmount.ToString();
    }

    public void SetTimeFreezeBoosters(int amount)
    {
        TimeFreezeBoostersAmount = amount;
        _timeFreezeBoosters.text = TimeFreezeBoostersAmount.ToString();
    }

    public void SetSpeedRocketBoosters(int amount)
    {
        SpeedRocketBoostersAmount = amount;
        _speedRocketBoosters.text = SpeedRocketBoostersAmount.ToString();
    }

    public void SetCryptoKeys(int amount)
    {
        CryptoKeysAmount = amount;
        _cryptoKeys.text = CryptoKeysAmount.ToString();
    }

    public void SetCryptoKeyFragments(int amount)
    {
        CryptoKeyFragments = amount;
        _cryptoKeyFragments.text = CryptoKeyFragments.ToString();
    }

    private void Start()
    {
        _freezeTimeButton.onClick.AddListener(OnFreezeTime);
        _speedRocketButton.onClick.AddListener(OnSpeedRocket);
    }

    private void OnFreezeTime()
    {
        _audioSource.Play();

        OnFreezeTimeClicked?.Invoke();
    }

    private void OnSpeedRocket()
    {
        _audioSource.Play();

        OnSpeedRocketClicked?.Invoke();
    }
}
