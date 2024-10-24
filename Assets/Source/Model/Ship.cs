using System;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Transformable, IUpdatable
{
    private const float DistanceTolerance = 0.05f;

    private Vector3 _refuelingPosition;
    private Vector3 _startPosition;
    private List<ShipTank> _tanks;
    private int _emptyTanks;
    private float _speed = 3f;

    public Ship(Vector3 shipWaitingPlace, ShipSetup shipSetup) : base(shipWaitingPlace, default)
    {
        Target = Position;

        _tanks = new List<ShipTank>(shipSetup.Tanks.Length);
        _emptyTanks = _tanks.Capacity;

        for (int i = 0; i < _tanks.Capacity; i++)
        {
            _tanks.Add(new ShipTank(shipSetup.Tanks[i].FuelType, shipSetup.Tanks[i].Size));
            _tanks[_tanks.Count - 1].Filled += OnTankFilled;
        }
    }

    public Ship(Vector3 shipWaitingPlace, ShipTank[] tanks) : base(shipWaitingPlace, default)
    {
        Target = Position;

        _tanks = new List<ShipTank>(tanks.Length);
        _emptyTanks = _tanks.Capacity;

        for (int i = 0; i < _tanks.Capacity; i++)
            _tanks.Add(new ShipTank(tanks[i].FuelType, tanks[i].Size));
    }

    public event Action StopedAtRefuelingPoint;
    public event Action<Ship> LeavedStation;
    public event Action<Ship> Refueled;

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

    public Vector3 Target { get; private set; }
    public List<ShipTank> Tanks => _tanks;
    public int EmptyTanks => _emptyTanks;

    public void Refuel(float amount, Fuel fuelType)
    {
        ShipTank tank = Tanks.Find(tank => tank.FuelType == fuelType && tank.IsFull == false);

        if (tank == null)
            throw new InvalidOperationException($"The ship does not have not full tanks with {fuelType} fuel.");

        tank.Refuel(amount, out float residue);
    }

    private void OnTankFilled(ShipTank shipTank)
    {
        shipTank.Filled -= OnTankFilled;

        Refueled?.Invoke(this);

        if (--_emptyTanks == 0)
            LeaveStation();
    }

    public float RequestFuelCount(Fuel fuel)
    {
        ShipTank tankToRefuel = Tanks.Find(tank => tank.FuelType == fuel && tank.IsFull == false);

        if (tankToRefuel == null)
            return 0;

        return tankToRefuel.Capacity - tankToRefuel.CurrentAmount;
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
