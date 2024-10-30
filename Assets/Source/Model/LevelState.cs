using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelState : IActivatable
{
    private UIMenu _levelCompleteWindow;
    private UIMenu _loseWindow;
    private UIMenu _pauseWindow;
    private Button _pauseButton;
    private List<Ship> _shipsQueue;
    private Station _station;
    private Timer _timer;

    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, UIMenu pauseWindow, Button pauseButton, Station station, List<Ship> shipsQueue, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _pauseWindow = pauseWindow;
        _pauseButton = pauseButton;
        _station = station;
        _shipsQueue = shipsQueue;
        _timer = timer;
    }

    public void Enable()
    {
        _pauseWindow.Hid += UnPause;
        _pauseButton.onClick.AddListener(Pause);
        _timer.Expired += OnTimerExpired;
        _station.PlaceFreed += OnStationPlaceFreed;
    }

    public void Disable()
    {
        _pauseWindow.Hid -= UnPause;
        _pauseButton.onClick.RemoveListener(Pause);
        _timer.Expired -= OnTimerExpired;
        _station.PlaceFreed -= OnStationPlaceFreed;
    }

    public bool IsGameOver { get; private set; } = false;
    public bool IsPaused { get; private set; } = Time.timeScale == 0f;
    protected List<Ship> ShipsQueue => _shipsQueue;
    protected Timer Timer => _timer;

    public void Pause()
    {
        _pauseButton.gameObject.SetActive(false);
        Time.timeScale = 0f;
        _pauseWindow.Show();
    }

    public void UnPause()
    {
        _pauseButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    protected virtual void OnStationPlaceFreed(Ship ship)
    {
        LetShipOnStation();
    }

    protected void LetShipOnStation()
    {
        if (_shipsQueue.Count > 0)
        {
            _station.Arrive(_shipsQueue[0]);
            _shipsQueue.RemoveAt(0);
        }
        else if (_station.ActiveShipCount == 0)
        {
            _timer.Stop();
            _levelCompleteWindow.Show();
            _pauseButton.gameObject.SetActive(false);
        }
    }

    private void OnTimerExpired()
    {
        IsGameOver = true;
        _loseWindow.Show();
        _pauseButton.gameObject.SetActive(false);
    }
}
