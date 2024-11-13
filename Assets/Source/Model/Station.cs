using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Station : IActivatable
{
    private FuelProvider _fuelProvider;
    private Grid _grid;
    private Ship[] _ships;
    private Transform[] _refuelingPoints;
    private Vector3[] _startPositions;

    public Station(Transform[] refuelingPoints, Vector3[] startPositions, Grid grid, TankContainer tankContainer)
    {
        _refuelingPoints = refuelingPoints;
        _ships = new Ship[refuelingPoints.Length];
        _startPositions = startPositions;
        _grid = grid;
        _fuelProvider = new FuelProvider(_grid, this, tankContainer);
    }

    public event Action<Ship> PlaceFreed;

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
    public FuelProvider FuelProvider => _fuelProvider;
    public int ActiveShipCount { get; private set; } = 0;

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

        ActiveShipCount++;

        _fuelProvider.RemoveSoftlock();

        ship.TankRefueled += _fuelProvider.StopRefueling;
        ship.LeavedStation += FreeRefuelingPoint;
        ship.StoppedAtRefuelingPoint += _fuelProvider.TryRefuel;
    }

    private void FreeRefuelingPoint(Ship ship)
    {
        ship.TankRefueled -= _fuelProvider.StopRefueling;
        ship.LeavedStation -= FreeRefuelingPoint;
        ship.StoppedAtRefuelingPoint -= _fuelProvider.TryRefuel;

        _ships[Array.IndexOf(_ships, ship)] = null;

        ActiveShipCount--;

        PlaceFreed?.Invoke(ship);
    }
}
