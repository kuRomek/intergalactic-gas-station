using System;
using System.Collections.Generic;

public class FuelProvider : IActivatable
{
    private Grid _grid;
    private Station _station;
    private TankContainer _tanks;
    private bool _providing = false;

    public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
    {
        _grid = grid;
        _station = station;
        _tanks = tankContainer;
    }

    public void Enable()
    {
        _tanks.TankEmptied += StopProviding;
    }

    public void Disable()
    {
        _tanks.TankEmptied -= StopProviding;
    }

    public void TryRefuel()
    {
        if (_providing)
            return;

        for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
        {
            try
            {
                if (_station.Ships[i].Position != _station.RefuelingPoints[i].position)
                    continue;

                if (DFSToFuelSource(_grid.RefuelingPoints[i], _tanks.Peek().FuelType))
                    Refuel(_station.Ships[i]);
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

    public void StopProviding()
    {
        _providing = false;
        TryRefuel();
        RemoveSoftLock();
    }

    private void Refuel(Ship ship)
    {
        Fuel requestedFuel = _tanks.Peek().FuelType;
        float requestedAmount = ship.RequestFuelCount(requestedFuel);

        if (requestedAmount == 0)
            return;

        _tanks.Peek().TakeFuel(requestedAmount, out float resultAmount);
        ship.Refuel(resultAmount, requestedFuel);

        _providing = true;
    }

    private bool CheckSoftLock()
    {
        if (_tanks.Count <= 1 || _station.ActiveShipCount <= 1)
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

    private bool DFSToFuelSource(IGridMember gridCell, Fuel fuel)
    {
        if (gridCell == null) 
            return false;

        PipeTemplate pipeTemplate = gridCell as PipeTemplate;

        if (pipeTemplate.ConnectedTemplates.Count == 0 || 
            (pipeTemplate.FuelType != fuel && pipeTemplate.FuelType != Fuel.Default))
            return false;

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
