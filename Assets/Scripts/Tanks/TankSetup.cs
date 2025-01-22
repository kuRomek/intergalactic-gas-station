using System;
using UnityEngine;
using Fuel;

namespace Tanks
{
    [Serializable]
    public struct TankSetup
    {
        [SerializeField] private Size _size;
        [SerializeField] private FuelType _fuelType;

        public Size Size => _size;

        public FuelType FuelType => _fuelType;
    }
}
