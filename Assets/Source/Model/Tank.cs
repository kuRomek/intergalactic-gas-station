using System;
using UnityEngine;
using static ITank;

public class Tank : Transformable, ITank
{
    private Size _size;
    private Fuel _fuelType;
    private int _currentAmount;

    public Tank(Vector3 position, Size size, Fuel fuelType) : base(position, default)
    {
        _size = size;
        _currentAmount = (int)_size;
        _fuelType = fuelType;

        ScaleTo(new Vector3(1f, (float)_size / MaximumSize, 1f));
    }

    public event Action<Tank> Emptied;
    public event Action FuelAmountChanged;
    public event Action<Fuel, int> FuelDecreased;
    public event Action<float> AmountChanging;

    public Fuel FuelType => _fuelType;
    public int Capacity => (int)_size;
    public int CurrentAmount => _currentAmount;

    public void TakeFuel(int requestedAmount, out int resultAmount)
    {
        resultAmount = (int)MathF.Min(requestedAmount, _currentAmount);
        AmountChanging?.Invoke(resultAmount);

        _currentAmount -= resultAmount;
        FuelAmountChanged?.Invoke();
        FuelDecreased?.Invoke(FuelType, resultAmount);

        if (_currentAmount == 0)
            Emptied?.Invoke(this);
    }
}
