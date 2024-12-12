using System;

public interface ITank
{
    const int MaximumSize = 6;

    event Action FuelAmountChanged;

    public enum Size
    {
        Big = MaximumSize,
        Medium = MaximumSize / 2,
        Small = MaximumSize / 3,
    }

    Fuel FuelType { get; }

    float Capacity { get; }

    float CurrentAmount { get; }

    void OnFuelProvidingStopped();
}