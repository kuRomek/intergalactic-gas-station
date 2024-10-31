using System;
using System.Collections.Generic;

public class FuelProvider : IActivatable
{
    private Grid _grid;
    private Station _station;
    private TankContainer _tanks;
    private SoftlockHandler _softlockHandler;
    private List<PipeTemplate> _path = null;
    private bool _isRefueling = false;

    public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
    {
        _grid = grid;
        _station = station;
        _tanks = tankContainer;
        _softlockHandler = new SoftlockHandler(_tanks, _station);
    }

    public void Enable()
    {
        _tanks.TankEmptied += StopRefueling;
        _tanks.StoppedShifting += TryRefuel;
    }

    public void Disable()
    {
        _tanks.TankEmptied -= StopRefueling;
        _tanks.StoppedShifting -= TryRefuel;
    }

    public void TryRefuel()
    {
        if (_isRefueling)
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
                    if (TryRefuel(_station.Ships[i]) == true)
                        break;
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is ArgumentException || exception is NullReferenceException)
            { }
        }
    }

    public void RemoveSoftlock() =>
        _softlockHandler.RemoveSoftlock();

    public void StopRefueling()
    {
        if (_path != null)
        {
            foreach (PipeTemplate pipeTemplate in _path)
                pipeTemplate.OnProvidingStopped();
        }

        _path = null;

        _softlockHandler.RemoveSoftlock();

        _isRefueling = false;

        TryRefuel();
    }

    private bool TryRefuel(Ship ship)
    {
        Fuel requestedFuel = _tanks.Peek().FuelType;
        float requestedAmount = ship.RequestFuelCount(requestedFuel);

        if (requestedAmount == 0)
            return false;

        _tanks.Peek().TakeFuel(requestedAmount, out float resultAmount);
        ship.Refuel(resultAmount, requestedFuel);

        _isRefueling = true;

        foreach (PipeTemplate pipe in _path)
            pipe.OnProvidingFuel();

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

        _path = new List<PipeTemplate>();
        List<PipeTemplate> checkedTemplates = new List<PipeTemplate>();
        Stack<PipeTemplate> templatesToCheck = new Stack<PipeTemplate>();

        templatesToCheck.Push(pipeTemplate);
        _path.Add(pipeTemplate);

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
                {
                    templatesToCheck.Push(connectedTemplate);
                    _path.Add(connectedTemplate);
                }
            }
        }
        while (templatesToCheck.Count > 0);

        _path = null;

        return false;
    }
}
