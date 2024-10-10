using System;
using System.Collections.Generic;
using System.Linq;

public class FuelProvider : IActivatable
{
    private Grid _grid;
    private Station _station;
    private TankContainer _tanks;

    public FuelProvider(Grid grid, Station station, TankContainer tankContainer)
    {
        _grid = grid;
        _station = station;
        _tanks = tankContainer;

        _tanks.TankEmptied += Unsubscribe;

        foreach (Tank tank in _tanks)
            tank.Emptied += TryRefuel;
    }

    private void Unsubscribe(Tank tank)
    {
        tank.Emptied -= TryRefuel;
    }

    public void Enable()
    {
    }

    public void Disable()
    {
        _tanks.TankEmptied -= Unsubscribe;
    }

    public void TryRefuel()
    {
        for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
        {
            try
            {
                if (_grid.RefuelingPoints[i] is PipeTemplate pipeTemplate && pipeTemplate.ConnectedTemplates.Count > 0 && DFSToFuelSource(pipeTemplate, _tanks.Peek().FuelType) == true)
                {
                    ShipTank tankToRefuel = _station.Ships[i].Tanks.FirstOrDefault(tank => tank.FuelType == _tanks.Peek().FuelType && tank.IsFull == false);
                    int refuelingAmount = (int)MathF.Min(_tanks.Peek().CurrentAmount, tankToRefuel.Capacity - tankToRefuel.CurrentAmount);
                    _station.Refuel(_station.Ships[i], tankToRefuel, refuelingAmount, out int residue);
                    _tanks.Peek().TakeFuel(refuelingAmount - residue);
                    return;
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is ArgumentException || exception is NullReferenceException)
            { }
        }
    }

    private bool DFSToFuelSource(PipeTemplate pipeTemplate, Fuel fuel)
    {
        if (pipeTemplate.FuelType != fuel)
            throw new InvalidOperationException("Pipes and fuel don't match.");

        List<PipeTemplate> checkedTemplates = new List<PipeTemplate>();
        Stack<PipeTemplate> templatesToCheck = new Stack<PipeTemplate>();
        templatesToCheck.Push(pipeTemplate);
        fuel = pipeTemplate.FuelType;
        PipeTemplate checkingTemplate;

        do
        {
            checkingTemplate = templatesToCheck.Pop();
            
            if (checkingTemplate == _grid.FuelSourcePoint)
                return true;

            checkedTemplates.Add(checkingTemplate);

            foreach (PipeTemplate connectedTemplate in checkingTemplate.ConnectedTemplates)
            {
                if (checkedTemplates.Find(template => template == connectedTemplate) == null && connectedTemplate.FuelType == fuel)
                    templatesToCheck.Push(connectedTemplate);
            }
        }
        while (templatesToCheck.Count > 0);

        return false;
    }
}
