using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteLevelState : LevelState
{
    private const int ShipCountForNewTanks = 3;

    private TankContainer _tanks;
    private RandomShipGenerator _randomShipGenerator;
    private RandomTankGenerator _randomTankGenerator;

    public InfiniteLevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, UIMenu pauseWindow, Button pauseButton, TankContainer tanks, Vector3 shipsWaitingPlace, PresenterFactory presenterFactory, Station station, Timer timer) :
        base(levelCompleteWindow, loseWindow, pauseWindow, pauseButton, station, new List<Ship>(ShipCountForNewTanks + 1), timer)
    {
        _tanks = tanks;
        _randomShipGenerator = new RandomShipGenerator(presenterFactory, shipsWaitingPlace);
        _randomTankGenerator = new RandomTankGenerator(_tanks);

        for (int i = 0; i < ShipCountForNewTanks; i++)
            AddShipToQueue();

        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }

    protected override void OnStationPlaceFreed(Ship ship)
    {
        AddShipToQueue();
        LetShipOnStation();

        if (ship.Tanks.Count == 1)
            Timer.AddTime(10f);
        else if (ship.Tanks.Count == 2)
            Timer.AddTime(20f);
        else if (ship.Tanks.Count == 3)
            Timer.AddTime(30f);
    }

    private void AddShipToQueue()
    {
        ShipsQueue.Add(_randomShipGenerator.Generate(Timer.SecondsPassed));

        if (_randomShipGenerator.GeneratedShips % ShipCountForNewTanks == 0)
            _randomTankGenerator.GenerateTanks();
    }
}
