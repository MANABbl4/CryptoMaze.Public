using CryptoMaze.Client;
using CryptoMaze.ClientServer.Game.Responses;
using CryptoMaze.Common;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private ResourcePanel _resourcePanel;

    [SerializeField]
    private BoostersPanel _boostersPanel;

    [SerializeField]
    private Button _freeEnergyButton;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private Button _playTestButton;

    [SerializeField]
    private Button _walletButton;

    [SerializeField]
    private Button _settingsButton;

    [SerializeField]
    private int _firstLabirintSceneId;


    private CryptoMazeClient _cryptoMazeClient;
    private PlayerDataResponse _playerData = null;

    void Start()
    {
        _cryptoMazeClient = SceneHelper.CreateAndValidateClient();

        SceneHelper.GetAndUpdatePlayerData(_cryptoMazeClient, _resourcePanel, _boostersPanel)
            .Then(playerData =>
            {
                _playerData = playerData;

                if (!playerData.lastGameFinished)
                {
                    SceneManager.LoadScene(_firstLabirintSceneId);
                }
            });

        _playButton.onClick.AddListener(OnPlay);
        _playTestButton.onClick.AddListener(() => { _audioSource.Play(); });
        _walletButton.onClick.AddListener(() => { _audioSource.Play(); });
        _settingsButton.onClick.AddListener(() =>
        {
            _audioSource.Play();
            PlayerPrefs.SetString("accessToken", string.Empty);
            PlayerPrefs.SetString("refreshToken", string.Empty);

            SceneManager.LoadScene(0);
        });

        _freeEnergyButton.onClick.AddListener(OnGetFreeEnergy);
    }

    private void OnPlay()
    {
        if (_playerData != null)
        {
            _audioSource.Play();

            var energyItem = _playerData.items.FirstOrDefault(x => x.type == (int)ItemType.Energy);

            if (energyItem.amount >= _playerData.energyStartGameNeeded)
            {
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        _cryptoMazeClient.Game
            .StartGame()
            .Then(result =>
            {
                if (result.started)
                {
                    var energyItem = _playerData.items.FirstOrDefault(x => x.type == (int)ItemType.Energy);
                    energyItem.amount -= result.energySpent;
                    _resourcePanel.SetEnergy(energyItem.amount);

                    SceneManager.LoadScene(_firstLabirintSceneId);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnGetFreeEnergy()
    {
        _audioSource.Play();

        _cryptoMazeClient.Game
            .AddEnergy()
            .Then(result =>
            {
                var energyItem = _playerData.items.FirstOrDefault(x => x.type == (int)ItemType.Energy);
                energyItem.amount = result.currentEnergyAmount;

                _resourcePanel.SetEnergy(result.currentEnergyAmount);
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }
}
