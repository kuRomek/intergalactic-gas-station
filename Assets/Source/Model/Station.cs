using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Station : Transformable, IActivatable
{
    private FuelProvider _fuelProvider;
    private Grid _grid;
    private Ship[] _ships;
    private Transform[] _refuelingPoints;
    private Vector3[] _startPositions;

    public Station(Transform[] refuelingPoints, Vector3[] startPositions, Grid grid, TankContainer tankContainer) : base(default, default)
    {
        _refuelingPoints = refuelingPoints;
        _ships = new Ship[refuelingPoints.Length];
        _startPositions = startPositions;
        _grid = grid;
        _fuelProvider = new FuelProvider(_grid, this, tankContainer);
    }

    public event Action PlaceFreed;

    public void Enable()
    {
        _fuelProvider.Enable();
        _grid.PipelineChanged += _fuelProvider.TryRefuel;
    }

    public void Disable()
    {
        _fuelProvider.Disable();
        _grid.PipelineChanged -= _fuelProvider.TryRefuel;
    }

    public Ship[] Ships => _ships;
    public Transform[] RefuelingPoints => _refuelingPoints;
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

        _fuelProvider.RemoveSoftLock();

        ship.Refueled += _fuelProvider.OnRefueled;

        ship.LeavedStation += FreeRefuelingPoint;
        ship.StopedAtRefuelingPoint += _fuelProvider.TryRefuel;
    }

    private void FreeRefuelingPoint(Ship ship)
    {
        ship.LeavedStation -= FreeRefuelingPoint;
        ship.StopedAtRefuelingPoint -= _fuelProvider.TryRefuel;

        _ships[Array.IndexOf(_ships, ship)] = null;

        PlaceFreed?.Invoke();
    }
}
