using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fuel;
using StructureElements;

namespace Tanks
{
    public class TankContainer : IEnumerable<Tank>
    {
        private Queue<Tank> _tanks = new Queue<Tank>();
        private PresenterFactory _presenterFactory;
        private Vector3 _tanksPosition;
        private Tank _lastTank;
        private Dictionary<FuelType, float> _fuelCounts = new Dictionary<FuelType, float>();
        private float _distanceBetweenTanks = 0.25f;

        public TankContainer(Vector3 tanksPosition, PresenterFactory presenterFactory)
        {
            _tanksPosition = tanksPosition;
            _presenterFactory = presenterFactory;
        }

        public event Action<Vector3> FirstTankRemoved;

        public event Action TankEmptied;

        public event Action StoppedShifting;

        public int Count => _tanks.Count;

        public bool IsShifting { get; private set; } = false;

        public IEnumerator<Tank> GetEnumerator()
        {
            return _tanks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tanks.GetEnumerator();
        }

        public Tank Add(Size size, FuelType fuelType)
        {
            Tank newTank = new Tank(default, size, fuelType);
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

        public void RemoveTank(Tank tank)
        {
            _tanks.Dequeue();
            tank.Emptied -= RemoveTank;

            tank.Destroy();

            TankEmptied?.Invoke();

            if (_tanks.Count > 0)
            {
                Vector3 elevation = _tanksPosition +
                    (_tanks.Peek().Capacity / (int)Size.Big * Vector3.down) -
                    _tanks.Peek().Position;

                IsShifting = true;
                FirstTankRemoved?.Invoke(elevation);
            }
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

            Vector3 elevation =
                _tanksPosition + (_tanks.Peek().Capacity / (int)Size.Big * Vector3.down) - _tanks.Peek().Position;
            IsShifting = true;
            FirstTankRemoved?.Invoke(elevation);
        }

        public float GetCount(FuelType fuel)
        {
            if (_fuelCounts.TryGetValue(fuel, out float count))
                return count;
            else
                return 0;
        }

        public void StopShifting()
        {
            IsShifting = false;
            StoppedShifting?.Invoke();
        }

        private void PutToEnd(Tank tank)
        {
            if (_tanks.Count == 0)
            {
                tank.MoveTo(_tanksPosition + (Vector3.down * tank.Capacity / (int)Size.Big));
            }
            else
            {
                tank.MoveTo(_lastTank.Position +
                    (Vector3.down * ((_lastTank.Capacity / (int)Size.Big) +
                    (tank.Capacity / (int)Size.Big) +
                    _distanceBetweenTanks)));
            }

            _lastTank = tank;
        }

        private void DecreseAmount(FuelType fuel, float amount)
        {
            _fuelCounts[fuel] -= amount;
        }
    }
}
