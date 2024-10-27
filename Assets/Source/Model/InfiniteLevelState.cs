using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InfiniteLevelState : LevelState
{
    private const int ShipCountForNewTanks = 3;

    private TankContainer _tanks;
    private RandomShipGenerator _randomShipGenerator;
    private ITank.Size[] _sizes = (ITank.Size[])Enum.GetValues(typeof(ITank.Size));
    private Fuel[] _fuels = (Fuel[])Enum.GetValues(typeof(Fuel));

    public InfiniteLevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, UIMenu pauseWindow, Button pauseButton, TankContainer tanks, Vector3 shipsWaitingPlace, PresenterFactory presenterFactory, Station station, Timer timer) :
        base(levelCompleteWindow, loseWindow, pauseWindow, pauseButton, station, new List<Ship>(ShipCountForNewTanks + 1), timer)
    {
        _randomShipGenerator = new RandomShipGenerator(presenterFactory, shipsWaitingPlace);
        _tanks = tanks;

        for (int i = 0; i < ShipCountForNewTanks; i++)
            AddShipToQueue();

        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }

    protected override void OnStationPlaceFreed()
    {
        AddShipToQueue();
        LetShipOnStation();
        Timer.AddTime(5);
    }

    private void AddShipToQueue()
    {
        ShipsQueue.Add(_randomShipGenerator.Generate(0f));

        if (_randomShipGenerator.GeneratedShips % ShipCountForNewTanks == 0)
            GenerateTanks();
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
