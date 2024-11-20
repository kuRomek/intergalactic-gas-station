public class SoftlockHandler
{
    private TankContainer _tanks;
    private Station _station;

    public SoftlockHandler(TankContainer tanks, Station station)
    {
        _tanks = tanks;
        _station = station;
    }

    public void RemoveSoftlock()
    {
        int i = 0;

        while (CheckSoftLock() && ++i < 100)
            _tanks.PutFirstToEnd();

        if (i == 100)
            throw new System.Exception("Over 100 iterations on removing softlock");
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