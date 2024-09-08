using UnityEngine;

public class Station : Transformable
{
    private Ship _leftShip = null;
    private Ship _rightShip = null;
    private Ship _topShip = null;
    private Vector3 _leftRefuelingPoint;
    private Vector3 _rightRefuelingPoint;
    private Vector3 _topRefuelingPoint;

    public Station(Vector3 leftRefuelPoint, Vector3 rightRefuelPoint, Vector3 topRefuelPoint) : base(Vector3.zero, Quaternion.identity)
    {
        _leftRefuelingPoint = leftRefuelPoint;
        _rightRefuelingPoint = rightRefuelPoint;
        _topRefuelingPoint = topRefuelPoint;
    }

    public void Arrive(Ship ship)
    {
        if (ship.Target == _leftRefuelingPoint)
        {
            if (_leftShip != null)
                throw new System.InvalidOperationException("Left refueling point is already taken.");

            _leftShip = ship;
        }
        else if (ship.Target == _rightRefuelingPoint)
        {
            if (_rightShip != null)
                throw new System.InvalidOperationException("Right refueling point is already taken.");

            _rightShip = ship;
        }
        else if (ship.Target == _topRefuelingPoint)
        {
            if (_topShip != null)
                throw new System.InvalidOperationException("Top refueling point is already taken.");

            _topShip = ship;
        }
    }

    public void RefuelOnLeft(Fuel fuel)
    {
        _leftShip.Refuel(fuel);
    }

    public void RefuelOnRight(Fuel fuel)
    {
        _rightShip.Refuel(fuel);
    }

    public void RefuelOnTop(Fuel fuel)
    {
        _topShip.Refuel(fuel);
    }
}
