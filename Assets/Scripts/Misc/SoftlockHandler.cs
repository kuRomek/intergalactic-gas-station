using IntergalacticGasStation.LevelControl;
using IntergalacticGasStation.Ships;
using IntergalacticGasStation.Tanks;

namespace IntergalacticGasStation
{
    namespace Misc
    {
        public class SoftlockHandler
        {
            private TankContainer _tanks;
            private Station _station;
            private float _maximumIterations = 100;

            public SoftlockHandler(TankContainer tanks, Station station)
            {
                _tanks = tanks;
                _station = station;
            }

            public void RemoveSoftlock()
            {
                int i = 0;

                while (CheckSoftLock() && ++i < _maximumIterations)
                    _tanks.PutFirstToEnd();

                if (i == _maximumIterations)
                    throw new System.Exception($"Over {_maximumIterations} iterations on removing softlock");
            }

            private bool CheckSoftLock()
            {
                if (_tanks.Count <= 1 || _station.ShipOnRefuelingPointsCount < 1)
                    return false;

                foreach (Ship ship in _station.Ships)
                {
                    if (ship == null)
                        continue;

                    foreach (ShipTank tank in ship.Tanks)
                    {
                        if (tank.IsFull == false && tank.FuelType == _tanks.Peek().FuelType)
                            return false;
                    }
                }

                return true;
            }
        }
    }
}