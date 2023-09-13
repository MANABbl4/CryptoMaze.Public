using System;
using UnityEngine;
using UnityEngine.UI;

public class SeasonTimer : MonoBehaviour
{
    public Action OnSeasonFinished;

    [SerializeField]
    private Text _timerText;

    [SerializeField]
    private string _timerFormat = "dd\\D\\ hh\\h\\ mm\\m\\ ss\\s\\ ";

    private float _timer = 0f;

    public int TimeLeft { get; private set; } = 0;

    public void SetTimeLeft(int timeLeft)
    {
        TimeLeft = timeLeft;

        UpdateTimer();
    }

    private void UpdateTimer()
    {
        var timeSpan = TimeSpan.FromSeconds(TimeLeft);
        _timerText.text = timeSpan.ToString(_timerFormat);
    }

    private void Update()
    {
        if (TimeLeft > 0)
        {
            _timer += Time.deltaTime;

            if (_timer > 1f)
            {
                _timer -= 1f;
                TimeLeft -= 1;

                UpdateTimer();

                if (TimeLeft < 1)
                {
                    OnSeasonFinished?.Invoke();
                }
            }
        }
    }
}
