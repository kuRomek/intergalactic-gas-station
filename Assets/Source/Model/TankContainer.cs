using System.Collections.Generic;
using UnityEngine;

public class TankContainer
{
    private Queue<Tank> _tanks = new Queue<Tank>();
    private Vector3 _tanksPosition;
    private Tank _lastTank;

    public TankContainer(Vector3 tankPosition)
    {
        _tanksPosition = tankPosition;
    }

    public Tank Add(Tank.Type type, Fuel fuelType)
    {
        Tank newTank = new Tank(default, type, fuelType);

        if (_tanks.Count == 0)
            newTank.MoveTo(_tanksPosition + Vector3.down * newTank.Capacity / 6f);
        else
            newTank.MoveTo(_lastTank.Position + Vector3.down * (_lastTank.Capacity / 6f + newTank.Capacity / 6f + 0.25f));

        _lastTank = newTank;
        _tanks.Enqueue(_lastTank);

        _lastTank.Emptied += RemoveTank;

        return _lastTank;
    }

    public void RemoveTank()
    {
        Tank removingTank = _tanks.Dequeue();
        removingTank.Emptied -= RemoveTank;

        if (_tanks.Count > 0)
        {
            Vector3 elevation = (_tanksPosition + _tanks.Peek().Capacity / 6f * Vector3.down) - _tanks.Peek().Position;

            foreach (Tank tank in _tanks)
                tank.MoveTo(tank.Position + elevation);
        }

        removingTank.Destroy();
    }

    public Tank Peek()
    {
        if (_tanks.Count > 0)
            return _tanks.Peek();
        else
            return null;
    }
}
