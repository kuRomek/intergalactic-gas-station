using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    private const float SecondsInMinute = 60f;

    [SerializeField] private TextMeshProUGUI _secondsUI;
    [SerializeField] private TextMeshProUGUI _minutesUI;
    [SerializeField] private Image _timerWheel;

    private Timer _timer;
    private float _startSeconds;

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void OnDisable()
    {
        _timer.TimeChanged -= OnTimeChanged;
    }

    public void Init(Timer timer)
    {
        _timer = timer;
        _startSeconds = _timer.Seconds;

        _timer.TimeChanged += OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        _secondsUI.text = ((int)(_timer.Seconds % SecondsInMinute)).ToString("00");
        _minutesUI.text = ((int)(_timer.Seconds / SecondsInMinute)).ToString();
        _timerWheel.fillAmount = _timer.Seconds / _startSeconds;
    }
}
