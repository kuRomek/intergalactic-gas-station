using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="FuelTypes", menuName="Fuel/FuelTypes", order=51)]
public class FuelColors : ScriptableObject
{
    [SerializeField] private FuelCellView[] _fuelCells;

    public Material GetMaterialOf(Fuel fuel)
    {
        return _fuelCells.FirstOrDefault(cell => cell.Fuel == fuel).Material;
    }
}

[Serializable]
public class FuelCellView
{
    [SerializeField] private Fuel _fuel;
    [SerializeField] private Material _material;

    public Fuel Fuel => _fuel;
    public Material Material => _material;
}