using System;
using UnityEngine;

[CreateAssetMenu(fileName="Level", menuName="Level/LevelSetup", order=52)]
public class LevelSetup : ScriptableObject
{
    [SerializeField, Min(1)] private int _shipCount;
    [SerializeField] private TankCell[] _tanks;

    public int ShipCount => _shipCount;
    public TankCell[] Tanks => _tanks;
}

[Serializable]
public class TankCell
{
    [SerializeField] private Tank.Type _type;
    [SerializeField] private Fuel _fuelType;

    public Tank.Type Type => _type;
    public Fuel FuelType => _fuelType;
}
