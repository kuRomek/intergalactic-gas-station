using System;

public interface ITank
{
    const int MaximumSize = 6;

    enum Size
    {
        Big = MaximumSize,
        Medium = 3,
        Small = 2
    }

    event Action FuelAmountChanged;

    Fuel FuelType { get; }
    int Capacity { get; }
    int CurrentAmount { get; }
}