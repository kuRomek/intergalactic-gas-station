using System.Collections.Generic;
using System.Linq;

public class LevelState : IActivatable
{
    private UIMenu _levelCompleteWindow;
    private UIMenu _loseWindow;
    private TankContainer _tanks;
    private List<Ship> _shipsQueue;
    private Station _station;
    private Timer _timer;
    private List<Fuel> _fuelTypesOnLevel = new List<Fuel>();
    
    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, TankContainer tanks, List<Ship> shipsQueue, Station station, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _tanks = tanks;
        _shipsQueue = shipsQueue;
        _station = station;
        _timer = timer;

        _timer.Expired += OnTimerExpired;

        foreach (Tank tank in _tanks)
        {
            if (_fuelTypesOnLevel.Contains(tank.FuelType) == false)
                _fuelTypesOnLevel.Add(tank.FuelType);
        }

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
            if (_station.ActiveShipCount == 0)
            {
                _station.Arrive(_shipsQueue[0]);
                _shipsQueue.RemoveAt(0);
                return;
            }
            
            List<Fuel> lackingFuelTypes = new List<Fuel>(_fuelTypesOnLevel);

            foreach (Ship ship in _station.Ships)
            {
                if (ship == null)
                    continue;

                foreach (ShipTank shipTank in ship.Tanks)
                    lackingFuelTypes.Remove(shipTank.FuelType);
            }

            Ship shipToArrive = null;

            foreach (Fuel fuel in lackingFuelTypes)
            {
                if ((shipToArrive = _shipsQueue.Find(ship => ship.Tanks.Find(tank => tank.FuelType == fuel) != null)) != null)
                    break;
            }

            shipToArrive ??= _shipsQueue[0];

            _shipsQueue.Remove(shipToArrive);

            _station.Arrive(shipToArrive);
        }
        else if (_station.ActiveShipCount == 0)
        {
            _timer.Stop();
            _levelCompleteWindow.Show();
        }
    }
}
