using UnityEngine;
using StructureElements;

namespace Ships
{
    public class RandomShipGenerator
    {
        private const string SmallShipsPath = "Ships/Small";
        private const string MediumShipsPath = "Ships/Medium";
        private const string BigShipsPath = "Ships/Big";

        private PresenterFactory _presenterFactory;
        private Vector3 _shipsWaitingPlace;
        private Object[] _smallShips;
        private Object[] _mediumShips;
        private Object[] _bigShips;
        private float _bigShipSpawnMultiplier = 700f;
        private float _mediumShipSpawnMultiplier = 500f;

        public RandomShipGenerator(PresenterFactory presenterFactory, Vector3 shipsWaitingPlace)
        {
            _presenterFactory = presenterFactory;
            _shipsWaitingPlace = shipsWaitingPlace;
            _smallShips = Resources.LoadAll(SmallShipsPath);
            _mediumShips = Resources.LoadAll(MediumShipsPath);
            _bigShips = Resources.LoadAll(BigShipsPath);
        }

        public int GeneratedShips { get; private set; } = 0;

        public Ship Generate(float passedSeconds)
        {
            float chanceToSpawnBigShip =
                1 - (_bigShipSpawnMultiplier / (passedSeconds + _bigShipSpawnMultiplier));

            float chanceToSpawnMediumShip =
                (1 - (_mediumShipSpawnMultiplier / (passedSeconds + _mediumShipSpawnMultiplier))) *
                (1 - chanceToSpawnBigShip);

            float chanceToSpawnSmallShip = 1 - chanceToSpawnMediumShip - chanceToSpawnBigShip;

            float randomChance = Random.value;
            Ship randomShip;

            if (randomChance <= chanceToSpawnSmallShip)
            {
                randomShip =
                    new Ship(_shipsWaitingPlace, (ShipSetup)_smallShips[Random.Range(0, _smallShips.Length)]);
            }
            else if (randomChance > chanceToSpawnSmallShip &&
                randomChance <= chanceToSpawnSmallShip + chanceToSpawnMediumShip)
            {
                randomShip =
                    new Ship(_shipsWaitingPlace, (ShipSetup)_mediumShips[Random.Range(0, _mediumShips.Length)]);
            }
            else
            {
                randomShip = new Ship(_shipsWaitingPlace, (ShipSetup)_bigShips[Random.Range(0, _bigShips.Length)]);
            }

            GeneratedShips++;
            _presenterFactory.CreateShip(randomShip);

            return randomShip;
        }
    }
}
