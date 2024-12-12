using System;
using UnityEngine;

[Serializable]
public struct TankSetup
{
    [SerializeField] private ITank.Size _size;
    [SerializeField] private Fuel _fuelType;

    public ITank.Size Size => _size;

    public Fuel FuelType => _fuelType;
}
