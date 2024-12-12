using UnityEngine;

public class RandomShipGenerator
{
    private PresenterFactory _presenterFactory;
    private Vector3 _shipsWaitingPlace;
    private Object[] _smallShips;
    private Object[] _mediumShips;
    private Object[] _bigShips;

    public RandomShipGenerator(PresenterFactory presenterFactory, Vector3 shipsWaitingPlace)
    {
        _presenterFactory = presenterFactory;
        _shipsWaitingPlace = shipsWaitingPlace;
        _smallShips = Resources.LoadAll("Ships/Small");
        _mediumShips = Resources.LoadAll("Ships/Medium");
        _bigShips = Resources.LoadAll("Ships/Big");
    }

    public int GeneratedShips { get; private set; } = 0;

    public Ship Generate(float passedSeconds)
    {
        float chanceToSpawnBigShip = 1 - (700 / (passedSeconds + 700));
        float chanceToSpawnMediumShip = (1 - (500 / (passedSeconds + 500))) * (1 - chanceToSpawnBigShip);
        float chanceToSpawnSmallShip = 1 - chanceToSpawnMediumShip - chanceToSpawnBigShip;

        float randomChance = Random.Range(0f, 1f);
        Ship randomShip;

        if (randomChance <= chanceToSpawnSmallShip)
            randomShip = new Ship(_shipsWaitingPlace, (ShipSetup)_smallShips[Random.Range(0, _smallShips.Length)]);
        else if (randomChance > chanceToSpawnSmallShip && randomChance <= chanceToSpawnSmallShip + chanceToSpawnMediumShip)
            randomShip = new Ship(_shipsWaitingPlace, (ShipSetup)_mediumShips[Random.Range(0, _mediumShips.Length)]);
        else
            randomShip = new Ship(_shipsWaitingPlace, (ShipSetup)_bigShips[Random.Range(0, _bigShips.Length)]);

        GeneratedShips++;
        _presenterFactory.CreateShip(randomShip);

        return randomShip;
    }
}
