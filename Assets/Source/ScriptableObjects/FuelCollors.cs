using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="FuelTypes", menuName="Fuel/FuelTypes", order=51)]
public class FuelCollors : ScriptableObject
{
    [SerializeField] private FuelCell[] _fuelCells;

    public Color GetColorOf(Fuel fuel)
    {
        return _fuelCells.FirstOrDefault(cell => cell.Fuel == fuel).Color;
    }
}

[Serializable]
public class FuelCell
{
    [SerializeField] private Fuel _fuel;
    [SerializeField] private Color _color;

    public Fuel Fuel => _fuel;
    public Color Color => _color;
}