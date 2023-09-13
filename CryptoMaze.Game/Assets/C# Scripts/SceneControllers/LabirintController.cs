using CryptoMaze.Client;
using CryptoMaze.ClientServer.Game.DataContainers;
using CryptoMaze.ClientServer.Game.Requests;
using CryptoMaze.Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabirintController : MonoBehaviour
{
    [SerializeField]
    private PlayerController _playerController;

    [SerializeField]
    private CollectedBlocksPanel _collectedBlocks;

    [SerializeField]
    private InventoryPanel _inventory;

    [SerializeField]
    private Timer _timer;

    [SerializeField]
    private Dialog _openStorageDialog;

    [SerializeField]
    private LabirintResultDialog _labirintResultDialog;

    [SerializeField]
    private LabirintResultDialog _gameResultDialog;

    [SerializeField]
    private List<CryptoBlock> _cryptoBlocks;

    [SerializeField]
    private List<Energy> _energies;

    [SerializeField]
    private List<CryptoKeyFragment> _cryptoKeyFragments;
    
    [SerializeField]
    private AudioClip _clickFx;

    [SerializeField]
    private AudioClip _scoreFx;

    [SerializeField]
    private AudioSource _playerAudioSource;

    [SerializeField]
    private ParticleSystem _collectEffect;

    [SerializeField]
    private ParticleSystem _tonEffect;

    [SerializeField]
    private ParticleSystem _ethEffect;

    [SerializeField]
    private ParticleSystem _btcEffect;

    [SerializeField]
    private GameObject _rocketEffect;

    [SerializeField]
    private int _labirintOrderId;

    [SerializeField]
    private int _mainMenuSceneId;

    [SerializeField]
    private int _nextLabirintSceneId;

    private CryptoMazeClient _cryptoMazeClient;

    private void Start()
    {
        _playerController.OnBlockCollected += OnPlayerCollectedBlock;
        _playerController.OnCryptoKeyFragmentCollected += OnPlayerCollectedCryptoKeyFragment;
        _playerController.OnEnergyCollected += OnPlayerCollectedEnergy;
        _playerController.OnStorage += OnPlayerOpenStorage;
        _playerController.OnExit += OnPlayerExit;
        _playerController.OnButtonRed += OnPlayerButtonRed;

        _inventory.OnFreezeTimeClicked += OnFreezeTime;
        _inventory.OnSpeedRocketClicked += OnActivateSpeedRocket;

        _timer.OnTimeOut += OnTimeOut;

        _cryptoMazeClient = SceneHelper.CreateAndValidateClient();

        _labirintResultDialog.gameObject.SetActive(false);
        _gameResultDialog.gameObject.SetActive(false);

        SceneHelper.GetAndUpdatePlayerData(_cryptoMazeClient, _inventory);

        _cryptoMazeClient.Game
            .StartLabirint()
            .Then(gameData =>
            {
                if (gameData.started)
                {
                    if (_labirintOrderId == gameData.labirintOrderId)
                    {
                        _timer.SetTimer(gameData.timeToFinish);
                        if (gameData.timeFreezed)
                        {
                            _timer.FreezeTime();
                        }

                        if (gameData.speedRocketActivated)
                        {
                            _playerController.ActivateRocketSpeed();
                        }

                        UpdateEnergies(gameData.energies);
                        UpdateCryptoKeyFragments(gameData.cryptoKeyFragments);
                        UpdateCryptoBlocks(gameData.cryptoBlocks);
                    }
                    else
                    {
                        SceneManager.LoadScene(_nextLabirintSceneId);
                    }
                }
                else
                {
                    FinishLabirint();
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void UpdateEnergies(List<EnergyData> energies)
    {
        if (_energies != null)
        {
            for (int i = 0; i < _energies.Count; ++i)
            {
                _energies[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < energies.Count; ++i)
            {
                if (i >= _energies.Count)
                {
                    break;
                }

                _energies[i].SetId(energies[i].id);
                _energies[i].gameObject.SetActive(!energies[i].found);
            }
        }
    }

    private void UpdateCryptoKeyFragments(List<CryptoKeyFragmentData> cryptoKeyFragments)
    {
        if (_cryptoKeyFragments != null)
        {
            for (int i = 0; i < _cryptoKeyFragments.Count; ++i)
            {
                _cryptoKeyFragments[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < cryptoKeyFragments.Count; ++i)
            {
                if (i >= _cryptoKeyFragments.Count)
                {
                    break;
                }

                _cryptoKeyFragments[i].SetId(cryptoKeyFragments[i].id);
                _cryptoKeyFragments[i].gameObject.SetActive(!cryptoKeyFragments[i].found);
            }
        }
    }

    private void UpdateCryptoBlocks(List<CryptoBlockData> data)
    {
        if (_cryptoBlocks != null)
        {
            if (_cryptoBlocks.Count != data.Count)
            {
                Debug.LogWarning("Scene crypto blocks count isn't the same as server said.");
            }

            Dictionary<CryptoType, Queue<CryptoBlock>> cryptoBlocks = new Dictionary<CryptoType, Queue<CryptoBlock>>
            {
                { CryptoType.Btc, new Queue<CryptoBlock>() },
                { CryptoType.Eth, new Queue<CryptoBlock>() },
                { CryptoType.Ton, new Queue<CryptoBlock>() }
            };

            foreach (var block in _cryptoBlocks)
            {
                cryptoBlocks[block.BlockType].Enqueue(block);
            }

            for (int i = 0; i < data.Count; ++i)
            {
                if (cryptoBlocks[data[i].Type].Count > 0)
                {
                    var block = cryptoBlocks[data[i].Type].Dequeue();
                    block.SetId(data[i].id);
                    block.gameObject.SetActive(!data[i].found);
                }
            }
        }
    }

    private void OnTimeOut()
    {
        FinishLabirint();
    }

    private void OnPlayerCollectedBlock(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);

        var block = obj.GetComponent<CryptoBlock>();

        if (block.BlockType == CryptoType.Btc)
        {
            Instantiate(_btcEffect, obj.transform.position, Quaternion.identity);
            obj.SetActive(false);
        }
        else if (block.BlockType == CryptoType.Eth)
        {
            Instantiate(_ethEffect, obj.transform.position, Quaternion.identity);
            obj.SetActive(false);
        }
        else if (block.BlockType == CryptoType.Ton)
        {
            Instantiate(_tonEffect, obj.transform.position, Quaternion.identity);
            obj.SetActive(false);
        }

        _cryptoMazeClient.Game
            .CollectCryptoBlock(new CollectCryptoBlockRequest() { id = block.Id })
            .Then(result =>
            {
                if (result.collected)
                {
                    if (block.BlockType == CryptoType.Btc)
                    {
                        _collectedBlocks.AddBtcBlocks(result.collectedAmount);
                    }
                    else if (block.BlockType == CryptoType.Eth)
                    {
                        _collectedBlocks.AddEthBlocks(result.collectedAmount);
                    }
                    else if (block.BlockType == CryptoType.Ton)
                    {
                        _collectedBlocks.AddTonBlocks(result.collectedAmount);
                    }
                }
                else
                {
                    Debug.LogWarning(result.message);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnPlayerCollectedCryptoKeyFragment(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);

        var cryptoKeyFragment = obj.GetComponent<CryptoKeyFragment>();

        Instantiate(_collectEffect, obj.transform.position, Quaternion.identity);
        obj.SetActive(false);

        _cryptoMazeClient.Game
            .CollectCryptoKeyFragment(new CollectCryptoKeyFragmentRequest() { id = cryptoKeyFragment.Id })
            .Then(result =>
            {
                if (result.collected)
                {
                    _inventory.AddCryptoKeyFragments(result.collectedAmount);
                }
                else
                {
                    Debug.LogWarning(result.message);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnPlayerCollectedEnergy(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);

        var energy = obj.GetComponent<Energy>();

        Instantiate(_collectEffect, obj.transform.position, Quaternion.identity);
        obj.SetActive(false);

        _cryptoMazeClient.Game
            .CollectEnergy(new CollectEnergyRequest() { id = energy.Id })
            .Then(result =>
            {
                if (!result.collected)
                {
                    Debug.LogWarning(result.message);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnPlayerOpenStorage(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);
    }

    private void OnPlayerExit(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);

        FinishLabirint();
    }

    private void OnPlayerButtonRed(GameObject obj)
    {
        _playerAudioSource.PlayOneShot(_scoreFx);
    }

    private void OnFreezeTime()
    {
        _cryptoMazeClient.Game
            .UseFreezeTimeBooster()
            .Then(result =>
            {
                if (result.used)
                {
                    _timer.FreezeTime();
                    _inventory.RemoveTimeFreezeBoosters(1);
                }
                else
                {
                    Debug.LogWarning(result.message);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }

    private void OnActivateSpeedRocket()
    {
        _cryptoMazeClient.Game
            .UseSpeedRocketBooster()
            .Then(result =>
            {
                if (result.used)
                {
                    _playerController.ActivateRocketSpeed();
                    _inventory.RemoveSpeedRocketBoosters(1);
                }
                else
                {
                    Debug.LogWarning(result.message);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
            
            Instantiate(_rocketEffect, transform.position, Quaternion.identity);
            _rocketEffect.SetActive(true);
    }

    private void FinishLabirint()
    {
        _playerController.StopMoving();
        _timer.SetPause(true);

        _inventory.OnFreezeTimeClicked -= OnFreezeTime;
        _inventory.OnSpeedRocketClicked -= OnActivateSpeedRocket;
        _timer.OnTimeOut -= OnTimeOut;

        _cryptoMazeClient.Game
            .FinishLabirint()
            .Then(result =>
            {
                if (result.finished)
                {
                    if (result.canStartNextLabirint)
                    {
                        // show results dialog?
                        _labirintResultDialog.gameObject.SetActive(true);
                        _labirintResultDialog.SetResults(result.labirintBtcAmount, result.labirintEthAmount, result.labirintTonAmount);
                        _labirintResultDialog.OnDialogAction += () => { SceneManager.LoadScene(_nextLabirintSceneId); };
                    }
                    else
                    {
                        // show finish game scene or dialog?
                        _cryptoMazeClient.Game
                            .FinishGame()
                            .Then(gameResult =>
                            {
                                _gameResultDialog.gameObject.SetActive(true);
                                _gameResultDialog.SetResults(gameResult.totalBtcAmount, gameResult.totalEthAmount, gameResult.totalTonAmount);
                                _gameResultDialog.OnDialogAction += () => { SceneManager.LoadScene(_mainMenuSceneId); };
                            })
                            .Catch(ex =>
                            {
                                Debug.LogError(ex.ToString());
                            });
                    }
                }
                else
                {
                    Debug.LogWarning("Failed to finish labirint. Go to main menu.");
                    SceneManager.LoadScene(_mainMenuSceneId);
                }
            })
            .Catch(ex =>
            {
                Debug.LogError(ex.ToString());
            });
    }
}
