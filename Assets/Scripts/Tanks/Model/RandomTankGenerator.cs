using System;
using System.Collections.Generic;
using Fuel;
using Random = UnityEngine.Random;

namespace Tanks
{
    public class RandomTankGenerator
    {
        private const int MinFuelAmount = 20;

        private TankContainer _tanks;
        private Size[] _sizes = (Size[])Enum.GetValues(typeof(Size));
        private FuelType[] _fuels = (FuelType[])Enum.GetValues(typeof(FuelType));

        public RandomTankGenerator(TankContainer tanks)
        {
            _tanks = tanks;
        }

        public void GenerateTanks()
        {
            if (_tanks.IsShifting)
            {
                _tanks.StoppedShifting += GenerateTanks;
            }
            else
            {
                _tanks.StoppedShifting -= GenerateTanks;

                List<FuelType> lackingFuels = new List<FuelType>();

                foreach (FuelType fuel in _fuels)
                {
                    if (fuel == FuelType.Any)
                        continue;

                    if (_tanks.GetCount(fuel) < MinFuelAmount)
                        lackingFuels.Add(fuel);
                }

                while (lackingFuels.Count != 0)
                {
                    FuelType randomFuel = lackingFuels[Random.Range(0, lackingFuels.Count)];

                    _tanks.Add(_sizes[Random.Range(0, _sizes.Length)], randomFuel);

                    if (_tanks.GetCount(randomFuel) >= MinFuelAmount)
                        lackingFuels.Remove(randomFuel);
                }
            }
        }
    }
}
