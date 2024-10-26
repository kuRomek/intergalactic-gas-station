using System.Collections.Generic;

public class LevelState : IActivatable
{
    private UIMenu _levelCompleteWindow;
    private UIMenu _loseWindow;
    private List<Ship> _shipsQueue;
    private Station _station;
    private Timer _timer;

    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, Station station, List<Ship> shipsQueue, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _station = station;
        _shipsQueue = shipsQueue;
        _timer = timer;
    }

    public void Enable()
    {
        _timer.Expired += OnTimerExpired;
        _station.PlaceFreed += OnStationPlaceFreed;
    }

    public void Disable()
    {
        _timer.Expired -= OnTimerExpired;
        _station.PlaceFreed -= OnStationPlaceFreed;
    }

    public bool IsGameOver => _timer.IsRunning == false;
    protected List<Ship> ShipsQueue => _shipsQueue;
    protected Timer Timer => _timer;

    protected virtual void OnStationPlaceFreed()
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
        }
    }

    private void OnTimerExpired()
    {
        _timer.Stop();
        _loseWindow.Show();
    }
}
