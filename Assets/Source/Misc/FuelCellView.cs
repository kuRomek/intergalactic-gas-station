using System;
using UnityEngine;

[Serializable]
public class FuelCellView
{
    [SerializeField] private Fuel _fuel;
    [SerializeField] private Material _material;

    public Fuel Fuel => _fuel;

    public Material Material => _material;
}