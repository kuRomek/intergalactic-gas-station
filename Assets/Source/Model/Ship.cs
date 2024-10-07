using System;
using System.Linq;
using UnityEngine;

public class Ship : Transformable, IUpdatable
{
    private const float DistanceTolerance = 0.001f;

    private Vector3 _refuelingPosition;
    private Vector3 _startPosition;
    private ShipTank[] _tanks;
    private int _emptyTanks;
    private float _speed = 3f;

    public Ship(Transform shipQueue, Fuel[] fuelTypes) : base(shipQueue.position, default)
    {
        if (fuelTypes.Length > 3)
            throw new ArgumentException("Ship can take maximum 3 types of fuel.");

        Target = Position;

        _tanks = new ShipTank[fuelTypes.Length];
        _emptyTanks = fuelTypes.Length;

        for (int i = 0; i < fuelTypes.Length; i++)
            _tanks[i] = new ShipTank(fuelTypes[i]);
    }

    public event Action StopedAtRefuelingPoint;
    public event Action<Ship> LeavedStation;

    public Vector3 Target { get; private set; }

    public void Update(float deltaTime)
    {
        if (Position != Target)
        {
            MoveTo(Vector3.Lerp(Position, Target, _speed * deltaTime));

            if (Vector3.SqrMagnitude(_refuelingPosition - Position) <= DistanceTolerance)
                MoveTo(Target);

            if (Position == _refuelingPosition)
                StopedAtRefuelingPoint?.Invoke();
        }
    }

    public void Refuel(Fuel fuel)
    {
        ShipTank tankToRefuel = _tanks.FirstOrDefault(tank => tank.Fuel == fuel) ?? throw new ArgumentException("No corresponding tank to refuel.");
        tankToRefuel.Refuel();

        if (--_emptyTanks == 0)
            LeaveStation();
    }

    public void ArriveAtStation(Vector3 startPosition, Transform refuelingPoint)
    {
        _startPosition = startPosition;
        MoveTo(_startPosition);

        _refuelingPosition = refuelingPoint.position;

        RotateOn(refuelingPoint.rotation);

        Target = refuelingPoint.position;
    }

    private void LeaveStation()
    {
        Target += (Target - _startPosition) * 2f;

        LeavedStation?.Invoke(this);
    }
}
