using System;
using System.Linq;
using UnityEngine;

public class Ship : Transformable, IUpdatable
{
    private const float DistanceTolerance = 0.001f;

    private ShipTrajectory _trajectory;
    private ShipTank[] _tanks;
    private int _emptyTanks;

    public Ship(ShipTrajectory trajectory, Fuel[] fuelTypes) : base(trajectory.Start.position, trajectory.Start.rotation)
    {
        if (fuelTypes.Length > 3)
            throw new System.ArgumentException("Ship can take maximum 3 types of fuel.");

        _trajectory = trajectory;
        Target = _trajectory.RefuelingPoint.position;

        _tanks = new ShipTank[fuelTypes.Length];
        _emptyTanks = fuelTypes.Length;

        for (int i = 0; i < fuelTypes.Length; i++)
            _tanks[i] = new ShipTank(fuelTypes[i]);
    }

    public event Action<Ship> LeavedStation;

    public Vector3 Target { get; private set; }

    public void Update(float deltaTime)
    {
        if (Vector3.SqrMagnitude(Target - Position) > DistanceTolerance)
            MoveTo(Vector3.Lerp(Position, Target, deltaTime * 5));
        else
            MoveTo(Target);
    }

    public void Refuel(Fuel fuel)
    {
        ShipTank tankToRefuel = _tanks.FirstOrDefault(tank => tank.Fuel == fuel) ?? throw new System.ArgumentException("No corresponding tank to refuel.");
        tankToRefuel.Refuel();

        if (--_emptyTanks == 0)
            LeaveStation();
    }

    private void LeaveStation()
    {
        Target = _trajectory.Finish.position;
        LeavedStation?.Invoke(this);
    }
}
