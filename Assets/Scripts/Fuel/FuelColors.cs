using System;
using System.Linq;
using UnityEngine;

namespace IntergalacticGasStation
{
    namespace Fuel
    {
        [CreateAssetMenu(fileName="FuelTypes", menuName="Fuel/FuelTypes", order=51)]
        public class FuelColors : ScriptableObject
        {
            [SerializeField] private FuelCellView[] _fuelCells;

            public Material GetMaterialOf(FuelType fuel)
            {
                FuelCellView fuelCell = _fuelCells.FirstOrDefault(cell => cell.Fuel == fuel);

                if (fuelCell == null)
                    throw new NullReferenceException("Material not found");
                else
                    return fuelCell.Material;
            }
        }
    }
}
