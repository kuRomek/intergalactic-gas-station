using System;
using UnityEngine;
using Fuel;
using StructureElements;

namespace Tanks
{
    public class Tank : Transformable, ITank
    {
        private Size _size;
        private FuelType _fuelType;
        private float _currentAmount;

        public Tank(Vector3 position, Size size, FuelType fuelType)
            : base(position, default)
        {
            _size = size;
            _currentAmount = (float)_size;
            _fuelType = fuelType;
        }

        public event Action<Tank> Emptied;

        public event Action FuelAmountChanged;

        public event Action<FuelType, float> FuelDecreased;

        public event Action<float> AmountChanging;

        public FuelType FuelType => _fuelType;

        public float Capacity => (float)_size;

        public float CurrentAmount => _currentAmount;

        public void TakeFuel(float requestedAmount, out float resultAmount)
        {
            resultAmount = (float)MathF.Min(requestedAmount, _currentAmount);
            AmountChanging?.Invoke(resultAmount);

            _currentAmount -= resultAmount;
            FuelAmountChanged?.Invoke();
            FuelDecreased?.Invoke(FuelType, resultAmount);
        }

        public void OnFuelProvidingStopped()
        {
            if (_currentAmount == 0)
                Emptied?.Invoke(this);
        }
    }
}
