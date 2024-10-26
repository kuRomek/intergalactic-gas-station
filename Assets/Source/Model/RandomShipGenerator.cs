using UnityEngine;

public class RandomShipGenerator
{
    private PresenterFactory _presenterFactory;
    private Vector3 _shipsWaitingPlace;
    private Object[] _ships;

    public RandomShipGenerator(PresenterFactory presenterFactory, Vector3 shipsWaitingPlace)
    {
        _presenterFactory = presenterFactory;
        _shipsWaitingPlace = shipsWaitingPlace;
        _ships = Resources.LoadAll("Ships");
    }

    public int GeneratedShips { get; private set; } = 0;

    public Ship Generate(float dificulty)
    {
        if (dificulty < 0 || dificulty > 1)
            throw new System.ArgumentOutOfRangeException("Difficulty has to be in range [0, 1].");

        GeneratedShips++;

        Ship ship = new Ship(_shipsWaitingPlace, (ShipSetup)_ships[Random.Range(0, _ships.Length)]);
        _presenterFactory.CreateShip(ship);

        return ship;
    }
}
