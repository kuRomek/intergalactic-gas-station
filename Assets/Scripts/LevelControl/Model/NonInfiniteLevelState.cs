using System.Collections.Generic;
using UnityEngine.UI;
using IntergalacticGasStation.Ships;
using IntergalacticGasStation.UI;

namespace IntergalacticGasStation
{
    namespace LevelControl
    {
        public class NonInfiniteLevelState : LevelState
        {
            public NonInfiniteLevelState(
                UIMenu levelCompleteWindow,
                UIMenu loseWindow,
                UIMenu pauseWindow,
                Button pauseButton,
                Station station,
                List<Ship> shipsQueue,
                Timer timer)
                : base(levelCompleteWindow, loseWindow, pauseWindow, pauseButton, station, shipsQueue, timer)
            {
                for (int i = 0; i < station.Ships.Length; i++)
                    LetShipOnStation();
            }
        }
    }
}
