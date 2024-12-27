using System;
using UnityEngine;

namespace IntergalacticGasStation
{
    namespace Fuel
    {
        [Serializable]
        public class FuelCellView
        {
            [SerializeField] private FuelType _fuel;
            [SerializeField] private Material _material;

            public FuelType Fuel => _fuel;

            public Material Material => _material;
        }
    }
}