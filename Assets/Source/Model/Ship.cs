using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ship : Transformable, IUpdatable
{
    private const float DistanceTolerance = 0.05f;

    private Vector3 _refuelingPosition;
    private Vector3 _startPosition;
    private List<ShipTank> _tanks;
    private int _emptyTanks;
    private float _speed = 3f;

    public Ship(Transform shipQueue, ShipSetup shipSetup) : base(shipQueue.position, default)
    {
        Target = Position;

        _tanks = new List<ShipTank>(shipSetup.Tanks.Length);
        _emptyTanks = _tanks.Capacity;

        for (int i = 0; i < _tanks.Capacity; i++)
            _tanks.Add(new ShipTank(shipSetup.Tanks[i].FuelType, shipSetup.Tanks[i].Size));
    }

    public event Action TankRefueled;
    public event Action StopedAtRefuelingPoint;
    public event Action<Ship> LeavedStation;

    public Vector3 Target { get; private set; }
    public List<ShipTank> Tanks => _tanks;

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

    public void Refuel(int amount, Fuel fuelType)
    {
        List<ShipTank> tanks = Tanks.FindAll(tank => tank.FuelType == fuelType && tank.IsFull == false);

        if (tanks.Count == 0)
            throw new InvalidOperationException($"The ship does not have any tanks with {fuelType} fuel.");

        foreach (ShipTank tank in tanks)
        {
            tank.Refuel(amount);
            amount -= tank.Capacity;

            if (tank.IsFull)
            {
                if (--_emptyTanks == 0)
                    LeaveStation();
                else
                    TankRefueled?.Invoke();
            }

            if (amount <= 0)
                return;
        }
    }

    public int RequestFuelCount(Fuel fuel)
    {
        return Tanks.FindAll(tank => tank.FuelType == fuel && tank.IsFull == false).Sum(tank => tank.Capacity - tank.CurrentAmount);
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
