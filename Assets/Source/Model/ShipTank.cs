using System;
using static ITank;

public class ShipTank : ITank
{
    private Size _size;
    private int _currentAmount;

    public ShipTank(Fuel fuel, Size size)
    {
        FuelType = fuel;
        _size = size;
        _currentAmount = 0;
    }

    public event Action FuelAmountChanged;

    public Fuel FuelType { get; }
    public bool IsFull { get; private set; } = false;
    public int CurrentAmount => _currentAmount;
    public int Capacity => (int)_size;

    public void Refuel(int amount)
    {
        if (IsFull)
            throw new InvalidOperationException("Tank is already full.");

        _currentAmount += amount;

        if (_currentAmount > Capacity)
            throw new InvalidOperationException($"After refueling tank cannot contain more fuel than its capacity. {_currentAmount}/{Capacity}");

        FuelAmountChanged?.Invoke();

        if (_currentAmount >= Capacity)
            IsFull = true;
    }
}
