using System;
using UnityEngine;
using UnityEngine.UI;

public class LabirintResultDialog : MonoBehaviour
{
    public Action OnDialogAction;

    [SerializeField]
    private Text _btcResult;

    [SerializeField]
    private Text _ethResult;

    [SerializeField]
    private Text _tonResult;

    [SerializeField]
    private Button _actionButton;

    [SerializeField]
    private AudioSource _audioSource;

    public void SetResults(int btc, int eth, int ton)
    {
        _btcResult.text = btc.ToString();
        _ethResult.text = eth.ToString();
        _tonResult.text = ton.ToString();
    }

    private void Start()
    {
        _actionButton.onClick.AddListener(OnAction);
    }

    private void OnAction()
    {
        _audioSource.Play();

        OnDialogAction?.Invoke();
    }
}
