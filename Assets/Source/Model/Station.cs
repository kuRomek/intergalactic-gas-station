using System.Collections.Generic;
using UnityEngine;

public class Station : Transformable, IActivatable
{
    private FuelProvider _fuelProvider;
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
        _fuelProvider = new FuelProvider(_grid);
    }

    public void Arrive(Ship ship)
    {
        if (ship.Target == _leftRefuelingPoint)
        {
            if (_leftShip != null)
                throw new System.InvalidOperationException("Left refueling point is already taken.");

            _leftShip = ship;

            _leftShip.LeavedStation += FreeRefuelingPoint;
        }
        else if (ship.Target == _rightRefuelingPoint)
        {
            if (_rightShip != null)
                throw new System.InvalidOperationException("Right refueling point is already taken.");

            _rightShip = ship;

            _rightShip.LeavedStation += FreeRefuelingPoint;
        }
        else if (ship.Target == _topRefuelingPoint)
        {
            if (_topShip != null)
                throw new System.InvalidOperationException("Top refueling point is already taken.");

            _topShip = ship;

            _topShip.LeavedStation += FreeRefuelingPoint;
        }
    }

    private void FreeRefuelingPoint(Ship ship)
    {
        ship.LeavedStation -= FreeRefuelingPoint;

        if (ship == _leftShip)
            _leftShip = null;
        else if (ship == _topShip)
            _topShip = null;
        else if (ship == _rightShip)
            _rightShip = null;
    }

    public void Enable()
    {
        _grid.PipelineChanged += OnPipelineChanged;
    }

    public void Disable()
    {
        _grid.PipelineChanged -= OnPipelineChanged;    
    }

    private void OnPipelineChanged()
    {
        foreach (var path in _fuelProvider.TryFindPath())
        {
            if (path.Key == 0)
            {
                if (_leftShip != null)
                    RefuelOnLeft(path.Value);
            }
            else if(path.Key == 1)
            {
                if (_topShip != null)
                    RefuelOnTop(path.Value);
            }
            else if (path.Key == 2) 
            {
                if (_rightShip != null)
                    RefuelOnRight(path.Value);
            }
        } 
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
}
