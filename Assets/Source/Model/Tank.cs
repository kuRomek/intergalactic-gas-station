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

        ScaleTo(new Vector3(1f, _currentAmount / 6f, 1f));
    }

    public event Action Emptied;

    public Fuel FuelType => _fuelType;
    public int Capacity => (int)_size;
    public int CurrentAmount => _currentAmount;

    public void TakeFuel(int amount)
    {
        int takingAmount = (int)MathF.Min(_currentAmount, amount);

        _currentAmount -= takingAmount;

        if (_currentAmount == 0)
            Emptied?.Invoke();
    }
}
