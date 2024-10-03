using System.Collections.Generic;

public class FuelProvider
{
    private Grid _grid;

    public FuelProvider(Grid grid)
    {
        _grid = grid;
    }

    public Dictionary<int, Fuel> TryFindPath()
    {
        Dictionary<int, Fuel> pathes = new Dictionary<int, Fuel>();

        for (int i = 0; i < _grid.RefuelingPoints.Length; i++)
        {
            if (_grid.RefuelingPoints[i] is PipeTemplate pipeTemplate && pipeTemplate.ConnectedTemplates.Count > 0 && DFSToFuelSource(pipeTemplate, out Fuel fuel))
               pathes.Add(i, fuel); 
        }

        return pathes;
    }

    private bool DFSToFuelSource(PipeTemplate pipeTemplate, out Fuel fuel)
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
