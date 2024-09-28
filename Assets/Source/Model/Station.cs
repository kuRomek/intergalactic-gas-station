using System.Collections.Generic;
using UnityEngine;

public class Station : Transformable, IActivatable
{
    private Grid _grid;
    private Ship _leftShip = null;
    private Ship _rightShip = null;
    private Ship _topShip = null;
    private Vector3 _leftRefuelingPoint;
    private Vector3 _rightRefuelingPoint;
    private Vector3 _topRefuelingPoint;

    public Station(Vector3 leftRefuelPoint, Vector3 rightRefuelPoint, Vector3 topRefuelPoint, Grid grid) : base(default, default)
    {
        _leftRefuelingPoint = leftRefuelPoint;
        _rightRefuelingPoint = rightRefuelPoint;
        _topRefuelingPoint = topRefuelPoint;
        _grid = grid;
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

    public void Enable()
    {
        _grid.PipelineChanged += TryRefuel;
    }

    public void Disable()
    {
        _grid.PipelineChanged -= TryRefuel;
    }

    private void RefuelOnLeft(Fuel fuel)
    {
        _leftShip.Refuel(fuel);
    }

    private void RefuelOnRight(Fuel fuel)
    {
        _rightShip.Refuel(fuel);
    }

    private void RefuelOnTop(Fuel fuel)
    {
        _topShip.Refuel(fuel);
    }

    private void TryRefuel()
    {
        List<int[]> path;

        if ((path = TryFindPath(_grid.LeftRefuelingPoint, out Fuel fuel)) == null)
            RefuelOnLeft(fuel);
        else if ((path = TryFindPath(_grid.TopRefuelingPoint, out fuel)) == null)
            RefuelOnTop(fuel);
        else if ((path = TryFindPath(_grid.RightRefuelingPoint, out fuel)) == null)
            RefuelOnRight(fuel);
    }

    private List<int[]> TryFindPath(int[] destination, out Fuel fuel)
    {
        List<int[]> path = new List<int[]>();
        fuel = Fuel.Empty;
        return path;
    }
}
