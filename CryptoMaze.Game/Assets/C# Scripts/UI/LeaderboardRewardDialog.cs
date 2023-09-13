using CryptoMaze.ClientServer.Game.DataContainers;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardRewardDialog : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _okButton;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _goldDialogSprite;

    [SerializeField]
    private Sprite _silverDialogSprite;

    [SerializeField]
    private Sprite _bronzeDialogSprite;

    [SerializeField]
    private Sprite _standartDialogSprite;

    [SerializeField]
    private LeaderboardRewardScrollView _scrollView;

    public void SetRewardData(UserItemModel[] reward, int rank)
    {
        _scrollView.SetData(reward);

        if (rank == 1)
        {
            _image.sprite = _goldDialogSprite;
        }
        else if (rank == 2)
        {
            _image.sprite = _silverDialogSprite;
        }
        else if (rank == 3)
        {
            _image.sprite = _bronzeDialogSprite;
        }
        else
        {
            _image.sprite = _standartDialogSprite;
        }
    }

    private void Start()
    {
        _okButton.onClick.AddListener(OnClose);
        _cancelButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        _audioSource.Play();
        gameObject.SetActive(false);
    }
}
