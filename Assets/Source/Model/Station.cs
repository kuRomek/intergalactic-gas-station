using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Station : Transformable, IActivatable
{
    private const int ShipCount = 3;

    private FuelProvider _fuelProvider;
    private Grid _grid;
    private Ship[] _ships = new Ship[ShipCount];
    private Transform[] _refuelingPoints;
    private Vector3[] _startPositions;

    public Station(Transform[] refuelingPoints, Vector3[] startPositions, Grid grid, TankContainer tankContainer) : base(default, default)
    {
        _refuelingPoints = refuelingPoints;
        _startPositions = startPositions;
        _grid = grid;
        _fuelProvider = new FuelProvider(_grid, this, tankContainer);
    }

    public event Action PlaceFreed;

    public int ActiveShipCount => _ships.Where(ship => ship != null).Count();

    public void Arrive(Ship ship)
    {
        List<int> freeSpotsIndecies = new List<int>();

        for (int i = 0; i < _ships.Length; i++)
        {
            if (_ships[i] == null)
                freeSpotsIndecies.Add(i);
        }

        if (freeSpotsIndecies.Count == 0)
            throw new InvalidOperationException("All refueling places are taken.");

        int randomSpot = freeSpotsIndecies[Random.Range(0, freeSpotsIndecies.Count)];

        _ships[randomSpot] = ship;
        ship.ArriveAtStation(_startPositions[randomSpot], _refuelingPoints[randomSpot]);

        ship.LeavedStation += FreeRefuelingPoint;
        ship.StopedAtRefuelingPoint += _fuelProvider.TryRefuel;
    }

    public void Refuel(int refuelingSpot, Fuel fuel)
    {
        if (_ships[refuelingSpot].Position != _refuelingPoints[refuelingSpot].position)
            throw new InvalidOperationException("Ship is not on point yet.");

        _ships[refuelingSpot].Refuel(fuel);
    }

    public void Enable()
    {
        _grid.PipelineChanged += _fuelProvider.TryRefuel;
    }

    public void Disable()
    {
        _grid.PipelineChanged -= _fuelProvider.TryRefuel;    
    }

    private void FreeRefuelingPoint(Ship ship)
    {
        ship.LeavedStation -= FreeRefuelingPoint;
        ship.StopedAtRefuelingPoint -= _fuelProvider.TryRefuel;

        _ships[Array.IndexOf(_ships, ship)] = null;

        //_fuelProvider.TryRefuel();

        PlaceFreed?.Invoke();
    }
}
