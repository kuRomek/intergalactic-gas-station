using System.Collections.Generic;

public class LevelState : IActivatable
{
    private UIMenu _levelCompleteWindow;
    private UIMenu _loseWindow;
    private TankContainer _tanks;
    private List<Ship> _shipsQueue;
    private Station _station;
    private Timer _timer;
    
    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, TankContainer tanks, List<Ship> shipsQueue, Station station, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _tanks = tanks;
        _shipsQueue = shipsQueue;
        _station = station;
        _timer = timer;

        _timer.Expired += OnTimerExpired;

        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }

    private void OnTimerExpired()
    {
        _timer.Stop();
        _loseWindow.Show();
    }

    public void Enable()
    {
        _station.PlaceFreed += LetShipOnStation;
    }

    public void Disable()
    {
        _station.PlaceFreed -= LetShipOnStation;
    }

    private void LetShipOnStation()
    {
        if (_shipsQueue.Count > 0)
        {
            Fuel fuel = _tanks.Peek().FuelType;
            int fuelCount = _tanks.Peek().CurrentAmount;
            Ship ship = _shipsQueue.Find(ship => new List<ShipTank>(ship.Tanks).Find(tank => tank.FuelType == fuel) != null);

            ship ??= _shipsQueue[0];

            _shipsQueue.Remove(ship);

            _station.Arrive(ship);
        }
        else if (_station.ActiveShipCount == 0)
        {
            _timer.Stop();
            _levelCompleteWindow.Show();
        }
    }
}
