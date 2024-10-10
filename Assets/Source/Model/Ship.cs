using System;
using System.Linq;
using UnityEngine;

public class Ship : Transformable, IUpdatable
{
    private const float DistanceTolerance = 0.05f;

    private Vector3 _refuelingPosition;
    private Vector3 _startPosition;
    private ShipTank[] _tanks;
    private int _emptyTanks;
    private float _speed = 3f;

    public Ship(Transform shipQueue, ShipSetup shipSetup) : base(shipQueue.position, default)
    {
        Target = Position;

        _tanks = new ShipTank[shipSetup.Tanks.Length];
        _emptyTanks = _tanks.Length;

        for (int i = 0; i < _tanks.Length; i++)
            _tanks[i] = new ShipTank(shipSetup.Tanks[i].FuelType, shipSetup.Tanks[i].Size);
    }

    public event Action StopedAtRefuelingPoint;
    public event Action<Ship> LeavedStation;

    public Vector3 Target { get; private set; }
    public ShipTank[] Tanks => _tanks;

    public void Update(float deltaTime)
    {
        if (Position != Target)
        {
            if (Vector3.SqrMagnitude(Target - Position) <= DistanceTolerance)
                MoveTo(Vector3.MoveTowards(Position, Target, _speed / 3f * deltaTime));
            else
                MoveTo(Vector3.Lerp(Position, Target, _speed * deltaTime));

            if (Position == _refuelingPosition)
                StopedAtRefuelingPoint?.Invoke();
        }
    }

    public void Refuel(ShipTank shipTank, int amount, out int residue)
    {
        shipTank.Refuel(amount, out residue);

        if (shipTank.IsFull)
        {
            if (--_emptyTanks == 0)
                LeaveStation();
        }
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
