using System;
using Fuel;

namespace Tanks
{
    public interface ITank
    {
        event Action FuelAmountChanged;

        FuelType FuelType { get; }

        float Capacity { get; }

        float CurrentAmount { get; }

        void OnFuelProvidingStopped();
    }
}