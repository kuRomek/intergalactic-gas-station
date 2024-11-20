using System;
using static ITank;

public class ShipTank : ITank
{
    private Size _size;
    private float _currentAmount;

    public ShipTank(Fuel fuel, Size size)
    {
        FuelType = fuel;
        _size = size;
        _currentAmount = 0;
    }

    public event Action FuelAmountChanged;
    public event Action<ShipTank> Refueled;

    public Fuel FuelType { get; }
    public bool IsFull { get; private set; } = false;
    public float CurrentAmount => _currentAmount;
    public Size Size => _size;
    public float Capacity => (float)_size;

    public void Refuel(float amount, out float residue)
    {
        if (IsFull)
            throw new InvalidOperationException("Tank is already full.");

        float lackingAmount = Capacity - _currentAmount;

        if (amount > lackingAmount)
        {
            residue = amount - lackingAmount;
            _currentAmount += lackingAmount;
        }
        else
        {
            residue = 0;
            _currentAmount += amount;
        }

        FuelAmountChanged?.Invoke();
    }

    public void OnFuelProvidingStopped()
    {
        if (_currentAmount == Capacity)
        {
            IsFull = true;
            Refueled?.Invoke(this);
        }
    }
}
