using UnityEngine;
using UnityEngine.UI;
using System;

public class Timer : MonoBehaviour
{
    public Action OnTimeOut;

    [SerializeField]
    private Text _timeLeft;

    [SerializeField]
    private Image _freezeTimeImage;

    public bool Paused { get; private set; } = false;

    private float _timer = 0;
    private int _timerInt = 0;
    private bool _freezed = false;

    public void SetPause(bool pause)
    {
        Paused = pause;
    }

    public void SetTimer(float time)
    {
        _timer = time;
        _timerInt = (int)time;
        _timeLeft.text = _timerInt.ToString();
    }

    public void FreezeTime()
    {
        _freezed = true;
        _freezeTimeImage.gameObject.SetActive(_freezed);
    }

    private void Start()
    {
        _freezed = false;
        _freezeTimeImage.gameObject.SetActive(_freezed);
    }

    private void Update()
    {
        if (!Paused && !_freezed && _timer > 0f)
        {
            _timer -= Time.deltaTime;
            if (_timerInt != (int)_timer)
            {
                _timerInt = (int)_timer;
                _timeLeft.text = _timerInt.ToString();
            }

            if (_timer < 0f)
            {
                OnTimeOut?.Invoke();
            }
        }
    }
}
