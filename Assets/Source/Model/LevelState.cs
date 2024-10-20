using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelState : IActivatable
{
    private const int ShipCountForNewTanks = 3;

    private UIMenu _levelCompleteWindow;
    private UIMenu _loseWindow;
    private TankContainer _tanks;
    private List<Ship> _shipsQueue;
    private Station _station;
    private Timer _timer;
    private RandomShipGenerator _randomShipGenerator;
    private ITank.Size[] _sizes = (ITank.Size[])Enum.GetValues(typeof(ITank.Size));
    private Fuel[] _fuels = (Fuel[])Enum.GetValues(typeof(Fuel));

    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, TankContainer tanks, List<Ship> shipsQueue, Station station, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _tanks = tanks;
        _shipsQueue = shipsQueue;
        _station = station;
        _timer = timer;

        _timer.Expired += OnTimerExpired;

        IsGameInfinite = false;

        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }

    public LevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, TankContainer tanks, Vector3 shipsWaitingPlace, PresenterFactory presenterFactory, Station station, Timer timer)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _loseWindow = loseWindow;
        _tanks = tanks;
        _randomShipGenerator = new RandomShipGenerator(presenterFactory, shipsWaitingPlace);
        _shipsQueue = new List<Ship>(ShipCountForNewTanks + 1);
        _station = station;
        _timer = timer;

        _timer.Expired += OnTimerExpired;

        IsGameInfinite = true;

        for (int i = 0; i < ShipCountForNewTanks; i++)
            AddShipToQueue();

        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }

    public void Enable()
    {
        if (IsGameInfinite)
            _station.PlaceFreed += AddShipToQueue;

        _station.PlaceFreed += LetShipOnStation;
    }

    public void Disable()
    {
        if (IsGameInfinite)
            _station.PlaceFreed -= AddShipToQueue;

        _station.PlaceFreed -= LetShipOnStation;
    }

    public bool IsGameOver => _timer.IsRunning == false;
    public bool IsGameInfinite { get; private set; }

    private void OnTimerExpired()
    {
        _timer.Stop();
        _loseWindow.Show();
    }

    private void AddShipToQueue()
    {
        _shipsQueue.Add(_randomShipGenerator.Generate(0f));

        if (_randomShipGenerator.GeneratedShips % ShipCountForNewTanks == 0)
            GenerateTanks();
    }

    private void LetShipOnStation()
    {
        _timer.AddTime(5);

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

    private void GenerateTanks()
    {
        List<Fuel> lackingFuels = new List<Fuel>();

        foreach (Fuel fuel in _fuels)
        {
            if (fuel == Fuel.Default)
                continue;

            if (_tanks.GetCount(fuel) < 10)
                lackingFuels.Add(fuel);
        }

        while (lackingFuels.Count != 0)
        {
            Fuel randomFuel = lackingFuels[Random.Range(0, lackingFuels.Count)];

            _tanks.Add(_sizes[Random.Range(0, _sizes.Length)], randomFuel);

            if (_tanks.GetCount(randomFuel) >= 10)
                lackingFuels.Remove(randomFuel);
        }
    }
}
