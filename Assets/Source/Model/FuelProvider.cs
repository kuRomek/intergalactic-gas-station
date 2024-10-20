using System;
using System.Collections.Generic;

public class FuelProvider
{
    private Grid _grid;
    private Station _station;
    private TankContainer _tanks;

    public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
    {
        _grid = grid;
        _station = station;
        _tanks = tankContainer;
    }

    public void TryRefuel()
    {
        for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
        {
            try
            {
                if (_station.Ships[i].Position != _station.RefuelingPoints[i].position)
                    continue;

                if (_grid.RefuelingPoints[i] is PipeTemplate pipeTemplate && pipeTemplate.ConnectedTemplates.Count > 0 && 
                    DFSToFuelSource(pipeTemplate, _tanks.Peek().FuelType))
                {
                    int oldTankCount = _tanks.Count;
                    Refuel(_station.Ships[i]);

                    if (oldTankCount != _tanks.Count || _station.Ships[i].EmptyTanks != 0)
                        TryRefuel();
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is ArgumentException || exception is NullReferenceException)
            { }
        }
    }

    public void RemoveSoftLock()
    {
        while (CheckSoftLock())
            _tanks.PutFirstToEnd();
    }

    private void Refuel(Ship ship)
    {
        Fuel requestedFuel = _tanks.Peek().FuelType;
        int requestedAmount = ship.RequestFuelCount(requestedFuel);
        _tanks.Peek().TakeFuel(requestedAmount, out int resultAmount);
        ship.Refuel(resultAmount, requestedFuel);

        RemoveSoftLock();
    }

    private bool CheckSoftLock()
    {
        if (_station.ActiveShipCount == 0)
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

    private bool DFSToFuelSource(PipeTemplate pipeTemplate, Fuel fuel)
    {
        if (pipeTemplate.FuelType != fuel && pipeTemplate.FuelType != Fuel.Default)
            throw new InvalidOperationException("Pipes and fuel don't match.");

        List<PipeTemplate> checkedTemplates = new List<PipeTemplate>();
        Stack<PipeTemplate> templatesToCheck = new Stack<PipeTemplate>();

        templatesToCheck.Push(pipeTemplate);

        PipeTemplate checkingTemplate;

        do
        {
            checkingTemplate = templatesToCheck.Pop();
            
            if (checkingTemplate == _grid.FuelSourcePoint)
                return true;

            checkedTemplates.Add(checkingTemplate);

            foreach (PipeTemplate connectedTemplate in checkingTemplate.ConnectedTemplates)
            {
                if (checkedTemplates.Find(template => template == connectedTemplate) == null && (connectedTemplate.FuelType == fuel || connectedTemplate.FuelType == Fuel.Default))
                    templatesToCheck.Push(connectedTemplate);
            }
        }
        while (templatesToCheck.Count > 0);

        return false;
    }
}
