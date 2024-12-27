using System;
using UnityEngine;
using IntergalacticGasStation.Fuel;

namespace IntergalacticGasStation
{
    namespace Tanks
    {
        [Serializable]
        public struct TankSetup
        {
            [SerializeField] private ITank.Size _size;
            [SerializeField] private FuelType _fuelType;

            public ITank.Size Size => _size;

            public FuelType FuelType => _fuelType;
        }
    }
}
