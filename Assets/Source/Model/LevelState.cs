using System.Collections.Generic;

public class LevelState : IActivatable
{
    private UIMenu _levelCompleteWindow;
    private Queue<Ship> _shipsQueue = new Queue<Ship>();
    private Station _station;
    
    public LevelState(UIMenu levelCompleteWindow, Queue<Ship> shipsQueue, Station station)
    {
        _levelCompleteWindow = levelCompleteWindow;
        _shipsQueue = shipsQueue;
        _station = station;

        _station.Arrive(_shipsQueue.Dequeue());
        _station.Arrive(_shipsQueue.Dequeue());
        _station.Arrive(_shipsQueue.Dequeue());
    }

    public void Enable()
    {
        _station.PlaceFreed += LetShipOnStation;
    }

    public void Disable()
    {
        _station.PlaceFreed -= LetShipOnStation;
    }

    private void LetShipOnStation()
    {
        if (_shipsQueue.Count > 0)
            _station.Arrive(_shipsQueue.Dequeue());
        else if (_station.ActiveShipCount == 0)
            _levelCompleteWindow.Show();
    }
}
