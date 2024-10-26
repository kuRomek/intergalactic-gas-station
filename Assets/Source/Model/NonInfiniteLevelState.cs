using System.Collections.Generic;

public class NonInfiniteLevelState : LevelState
{
    public NonInfiniteLevelState(UIMenu levelCompleteWindow, UIMenu loseWindow, Station station, List<Ship> shipsQueue, Timer timer) :
        base(levelCompleteWindow, loseWindow, station, shipsQueue, timer) 
    {
        LetShipOnStation();
        LetShipOnStation();
        LetShipOnStation();
    }
}
