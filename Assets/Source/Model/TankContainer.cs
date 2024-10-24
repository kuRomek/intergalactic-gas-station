using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankContainer : IEnumerable<Tank>
{
    private Queue<Tank> _tanks = new Queue<Tank>();
    private PresenterFactory _presenterFactory;
    private Vector3 _tanksPosition;
    private Tank _lastTank;
    private Dictionary<Fuel, float> _fuelCounts = new Dictionary<Fuel, float>();

    public TankContainer(Vector3 tanksPosition, PresenterFactory presenterFactory)
    {
        _tanksPosition = tanksPosition;
        _presenterFactory = presenterFactory;
    }

    public event Action<Vector3> FirstTankRemoved;
    public event Action<Tank> TankEmptied;

    public IEnumerator<Tank> GetEnumerator()
    {
        return _tanks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _tanks.GetEnumerator();
    }

    public int Count => _tanks.Count;

    public Tank Add(ITank.Size type, Fuel fuelType)
    {
        Tank newTank = new Tank(default, type, fuelType);
        PutToEnd(newTank);

        _presenterFactory.CreateTank(newTank);

        _tanks.Enqueue(newTank);

        if (_fuelCounts.ContainsKey(newTank.FuelType))
            _fuelCounts[newTank.FuelType] += newTank.Capacity;
        else
            _fuelCounts[newTank.FuelType] = newTank.Capacity;

        newTank.FuelDecreased += DecreseAmount;
        newTank.Emptied += RemoveTank;

        return newTank;
    }

    private void DecreseAmount(Fuel fuel, float amount)
    {
        _fuelCounts[fuel] -= amount;
    }

    public void RemoveTank(Tank tank)
    {
        _tanks.Dequeue();
        tank.Emptied -= RemoveTank;

        if (_tanks.Count > 0)
        {
            Vector3 elevation = (_tanksPosition + _tanks.Peek().Capacity / 6f * Vector3.down) - _tanks.Peek().Position;
            FirstTankRemoved?.Invoke(elevation);
        }

        tank.Destroy();

        TankEmptied?.Invoke(tank);
    }

    public Tank Peek()
    {
        if (_tanks.Count > 0)
            return _tanks.Peek();
        else
            return null;
    }

    public void PutFirstToEnd()
    {
        if (Count == 0)
            return;

        Tank tank = _tanks.Dequeue();
        _tanks.Enqueue(tank);
        PutToEnd(tank);

        Vector3 elevation = (_tanksPosition + _tanks.Peek().Capacity / 6f * Vector3.down) - _tanks.Peek().Position;
        FirstTankRemoved?.Invoke(elevation);
    }

    public float GetCount(Fuel fuel)
    {
        if (_fuelCounts.TryGetValue(fuel, out float count))
            return count;
        else
            return 0;
    }

    private void PutToEnd(Tank tank)
    {
        if (_tanks.Count == 0)
            tank.MoveTo(_tanksPosition + Vector3.down * tank.Capacity / 6f);
        else
            tank.MoveTo(_lastTank.Position + Vector3.down * (_lastTank.Capacity / 6f + tank.Capacity / 6f + 0.25f));

        _lastTank = tank;
    }
}
