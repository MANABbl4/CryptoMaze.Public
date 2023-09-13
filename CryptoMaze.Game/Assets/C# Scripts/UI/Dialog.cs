using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Action<Dialog> OnDialogAction;

    [SerializeField]
    private Button _actionButton;

    [SerializeField]
    private Button _cancelButton;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private Button _moveButton;

    [SerializeField]
    private Vector3 _translateVector;

    void Start()
    {
        _actionButton.onClick.AddListener(OnAction);
        _cancelButton.onClick.AddListener(OnClose);
    }

    private void OnClose()
    {
        _audioSource.Play();
        _moveButton.transform.Translate(_translateVector);
        gameObject.SetActive(false);
    }

    private void OnAction()
    {
        _audioSource.Play();
        _moveButton.transform.Translate(_translateVector);
        OnDialogAction?.Invoke(this);
    }
}
