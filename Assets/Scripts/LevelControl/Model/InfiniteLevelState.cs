using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LevelGrid;
using Ships;
using StructureElements;
using Tanks;
using UI;

namespace LevelControl
{
    public class InfiniteLevelState : LevelState
    {
        private const int ShipCountForNewTanks = 3;
        private const int ShipCountForNewGrid = 5;
        private const float SecondsAddingMultiplier = 10f;

        private TankContainer _tanks;
        private RandomShipGenerator _randomShipGenerator;
        private RandomTankGenerator _randomTankGenerator;
        private GridChanger _gridChanger;

        public InfiniteLevelState(
            UIMenu levelCompleteWindow,
            UIMenu loseWindow,
            UIMenu pauseWindow,
            Button pauseButton,
            TankContainer tanks,
            Vector3 shipsWaitingPlace,
            PresenterFactory presenterFactory,
            GridChanger gridChanger,
            Station station,
            Timer timer)
            : base(
                  levelCompleteWindow,
                  loseWindow,
                  pauseWindow,
                  pauseButton,
                  station,
                  new List<Ship>(ShipCountForNewTanks + 1),
                  timer)
        {
            _tanks = tanks;
            _randomShipGenerator = new RandomShipGenerator(presenterFactory, shipsWaitingPlace);
            _randomTankGenerator = new RandomTankGenerator(_tanks);
            _gridChanger = gridChanger;

            for (int i = 0; i < ShipCountForNewTanks; i++)
                AddShipToQueue();

            LetShipOnStation();
            LetShipOnStation();
            LetShipOnStation();

            _gridChanger.Change();
        }

        protected override void OnStationPlaceFreed(Ship ship)
        {
            AddShipToQueue();
            base.OnStationPlaceFreed(ship);

            Timer.AddTime(ship.Tanks.Count * SecondsAddingMultiplier);
        }

        private void AddShipToQueue()
        {
            ShipsQueue.Add(_randomShipGenerator.Generate(Timer.SecondsPassed));

            if (_randomShipGenerator.GeneratedShips % ShipCountForNewTanks == 0)
                _randomTankGenerator.GenerateTanks();

            if ((RefueledShipCount + 1) % ShipCountForNewGrid == 0)
                _gridChanger.Change();
        }
    }
}
