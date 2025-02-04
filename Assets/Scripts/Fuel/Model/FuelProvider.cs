using System;
using System.Collections.Generic;
using LevelControl;
using LevelGrid;
using Misc;
using Pipes;
using Ships;
using StructureElements;
using Tanks;
using Grid = LevelGrid.Grid;

namespace Fuel
{
    public class FuelProvider : IActivatable
    {
        private Grid _grid;
        private Pathfinder _pathfinder;
        private Station _station;
        private TankContainer _tanks;
        private SoftlockHandler _softlockHandler;
        private List<PipeTemplate> _path = null;
        private bool _isRefueling = false;

        public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
        {
            _grid = grid;
            _pathfinder = new Pathfinder(grid);
            _station = station;
            _tanks = tankContainer;
            _softlockHandler = new SoftlockHandler(_tanks, _station);
        }

        public IReadOnlyList<PipeTemplate> Path => _path;

        public void Enable()
        {
            _tanks.TankEmptied += StopRefueling;
            _tanks.StoppedShifting += TryRefuel;
        }

        public void Disable()
        {
            _tanks.TankEmptied -= StopRefueling;
            _tanks.StoppedShifting -= TryRefuel;
        }

        public void TryRefuel()
        {
            if (_isRefueling)
                return;

            if (_tanks.IsShifting)
                return;

            _softlockHandler.RemoveSoftlock();

            for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
            {
                try
                {
                    if (_station.Ships[i].Position != _station.RefuelingPoints[i].position)
                        continue;

                    if (_pathfinder.DFSToFuelSource(_grid.RefuelingPoints[i], _tanks.Peek().FuelType, out _path))
                    {
                        if (TryRefuel(_station.Ships[i]) == true)
                            break;
                        else
                            _path = null;
                    }
                }
                catch (Exception exception)
                when (exception is InvalidOperationException ||
                exception is ArgumentException ||
                exception is NullReferenceException)
                {
                }
            }
        }

        public void StopRefueling()
        {
            if (_isRefueling == true)
            {
                foreach (PipeTemplate pipeTemplate in _path)
                    pipeTemplate.StopProvidingFuel();

                _path = null;

                _isRefueling = false;
            }
        }

        public void RemoveSoftlock() =>
            _softlockHandler.RemoveSoftlock();

        private bool TryRefuel(Ship ship)
        {
            FuelType requestedFuel = _tanks.Peek().FuelType;
            float requestedAmount = ship.RequestFuelCount(requestedFuel);

            if (requestedAmount == 0)
                return false;

            _isRefueling = true;

            _tanks.Peek().TakeFuel(requestedAmount, out float resultAmount);
            ship.Refuel(resultAmount, requestedFuel);

            foreach (PipeTemplate pipe in _path)
                pipe.ProvideFuel();

            return true;
        }
    }
}
