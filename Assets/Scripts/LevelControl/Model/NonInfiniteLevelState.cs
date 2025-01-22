using System.Collections.Generic;
using UnityEngine.UI;
using Ships;
using UI;

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
            List<Ship> ships,
            Timer timer)
            : base(levelCompleteWindow, loseWindow, pauseWindow, pauseButton, station, ships, timer)
        {
            for (int i = 0; i < station.Ships.Length; i++)
                LetShipOnStation();
        }
    }
}
