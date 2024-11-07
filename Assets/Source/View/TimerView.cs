using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class TimerView : MonoBehaviour
{
    private const float SecondsInMinute = 60f;

    [SerializeField] private TextMeshProUGUI _secondsLeftUI;
    [SerializeField] private TextMeshProUGUI _minutesLeftUI;
    [SerializeField] private TextMeshProUGUI _secondsPassedUI;
    [SerializeField] private TextMeshProUGUI _minutesPassedUI;
    [SerializeField] private TextMeshProUGUI _secondsRecordUI;
    [SerializeField] private TextMeshProUGUI _minutesRecordUI;
    [SerializeField] private Image _timerWheel;

    private Timer _timer;
    private float _startSeconds;

    private void OnEnable()
    {
        if (_timer != null)
            _timer.TimeChanged += OnTimeChanged;

        if (_secondsRecordUI != null && _minutesRecordUI != null)
        {
            _secondsRecordUI.text = ((int)(YandexGame.savesData.InfiniteGameRecord % SecondsInMinute)).ToString("00");
            _minutesRecordUI.text = ((int)(YandexGame.savesData.InfiniteGameRecord / SecondsInMinute)).ToString();
        }
    }

    private void OnDisable()
    {
        _timer.TimeChanged -= OnTimeChanged;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    public void Init(Timer timer)
    {
        _timer = timer;
        _startSeconds = _timer.SecondsLeft;

        _timer.TimeChanged += OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        _secondsLeftUI.text = ((int)(_timer.SecondsLeft % SecondsInMinute)).ToString("00");
        _minutesLeftUI.text = ((int)(_timer.SecondsLeft / SecondsInMinute)).ToString();

        if (_timerWheel != null)
            _timerWheel.fillAmount = _timer.SecondsLeft / _startSeconds;

        if (_secondsPassedUI != null && _minutesPassedUI != null)
        {
            _secondsPassedUI.text = ((int)(_timer.SecondsPassed % SecondsInMinute)).ToString("00");
            _minutesPassedUI.text = ((int)(_timer.SecondsPassed / SecondsInMinute)).ToString();
        }
    }
}
