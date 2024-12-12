using System.Collections.Generic;
using UnityEngine.UI;

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
        : base(levelCompleteWindow, loseWindow, pauseWindow, pauseButton,  station, shipsQueue, timer)
    {
        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }
}
