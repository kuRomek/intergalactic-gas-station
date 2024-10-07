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
                if (_grid.RefuelingPoints[i] is PipeTemplate pipeTemplate && pipeTemplate.ConnectedTemplates.Count > 0 && DFSToFuelSource(pipeTemplate, _tanks.Peek().FuelType) == true)
                {
                    _station.Refuel(i, _tanks.Peek().FuelType);
                    _tanks.Peek().TakeFuel();
                    return;
                }
            }
            catch (Exception exception) when (exception is InvalidOperationException || exception is ArgumentException || exception is NullReferenceException)
            { }
        }
    }

    private bool DFSToFuelSource(PipeTemplate pipeTemplate, Fuel fuel)
    {
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