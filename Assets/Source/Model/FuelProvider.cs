using System;
using System.Collections.Generic;

public class FuelProvider : IActivatable
{
    private Grid _grid;
    private Station _station;
    private TankContainer _tanks;
    private SoftlockHandler _softlockHandler;
    private bool _refueling = false;

    public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
    {
        _grid = grid;
        _station = station;
        _tanks = tankContainer;
        _softlockHandler = new SoftlockHandler(_tanks, _station);
    }

    public void Enable()
    {
        _tanks.TankEmptied += StopProviding;
        _tanks.StoppedShifting += TryRefuel;
    }

    public void Disable()
    {
        _tanks.TankEmptied -= StopProviding;
        _tanks.StoppedShifting -= TryRefuel;
    }

    public void TryRefuel()
    {
        if (_refueling)
            return;

        if (_tanks.IsShifting)
            return;

        for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
        {
            try
            {
                if (_station.Ships[i].Position != _station.RefuelingPoints[i].position)
                    continue;

                if (DFSToFuelSource(_grid.RefuelingPoints[i], _tanks.Peek().FuelType))
                {
                    Refuel(_station.Ships[i]);
                    break;
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is ArgumentException || exception is NullReferenceException)
            { }
        }
    }

    public void RemoveSoftlock() =>
        _softlockHandler.RemoveSoftlock();

    public void StopProviding()
    {
        if (_refueling == true)
        {
            _refueling = false;
            _softlockHandler.RemoveSoftlock();
            TryRefuel();
        }
    }

    private void Refuel(Ship ship)
    {
        Fuel requestedFuel = _tanks.Peek().FuelType;
        float requestedAmount = ship.RequestFuelCount(requestedFuel);

        if (requestedAmount == 0)
            return;

        _tanks.Peek().TakeFuel(requestedAmount, out float resultAmount);
        ship.Refuel(resultAmount, requestedFuel);

        _refueling = true;
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
