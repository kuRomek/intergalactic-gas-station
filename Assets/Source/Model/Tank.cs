using System;
using UnityEngine;

[Serializable]
public class Tank : Transformable
{
    [SerializeField] private Type _type;
    [SerializeField] private Fuel _fuelType;
    private int _capacity;
    private int _currentAmount;

    public Tank(Vector3 position, Type type, Fuel fuelType) : base(position, default)
    {
        _type = type;

        if (_type == Type.Big)
            _capacity = 6;
        else if (_type == Type.Medium)
            _capacity = 3;
        else if (_type == Type.Small)
            _capacity = 2;

        ScaleTo(new Vector3(1f, _capacity / 6f, 1f));

        _currentAmount = _capacity;
        _fuelType = fuelType;
    }

    public event Action Emptied;

    public enum Type
    {
        Big,
        Medium,
        Small
    }

    public Fuel FuelType => _fuelType;
    public int Capacity => _capacity;

    public void TakeFuel()
    {
        int takingAmount = (int)MathF.Min(_currentAmount, 2);

        _currentAmount -= takingAmount;

        if (_currentAmount == 0)
            Emptied?.Invoke();
    }
}
