using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName="Ship", menuName="Ship/Ship", order=53)]
public class ShipSetup : ScriptableObject
{
    [SerializeField] private TankSetup[] _tanks;

    public TankSetup[] Tanks => _tanks;

    private void OnValidate()
    {
        if (_tanks.Select(tank => (int)tank.Size).Sum() > ITank.MaximumSize)
            throw new InvalidOperationException($"Ships' tanks must contain only {ITank.MaximumSize} units of fuel.");
    }
}
