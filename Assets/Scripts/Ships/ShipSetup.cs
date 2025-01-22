using System;
using System.Linq;
using UnityEngine;
using Tanks;

namespace Ships
{
    [CreateAssetMenu(fileName = "Ship", menuName = "Ship/Ship", order = 53)]
    public class ShipSetup : ScriptableObject
    {
        [SerializeField] private TankSetup[] _tanks;

        private TankSetup[] _lastTanks;

        public TankSetup[] Tanks => _tanks;

        private void OnValidate()
        {
            if (_tanks.Select(tank => (int)tank.Size).Sum() > (int)Size.Big)
            {
                _tanks = new TankSetup[_lastTanks.Length];

                for (int i = 0; i < _tanks.Length; i++)
                    _tanks[i] = _lastTanks[i];

                throw new InvalidOperationException($"Ships' tanks must contain only {(int)Size.Big} units of fuel.");
            }

            _lastTanks = new TankSetup[_tanks.Length];

            for (int i = 0; i < _lastTanks.Length; i++)
                _lastTanks[i] = _tanks[i];
        }
    }
}
