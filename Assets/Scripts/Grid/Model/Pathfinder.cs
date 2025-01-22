using System.Collections.Generic;
using System.Linq;
using Fuel;
using Pipes;

namespace LevelGrid
{
    public class Pathfinder
    {
        private Grid _grid;

        public Pathfinder(Grid grid)
        {
            _grid = grid;
        }

        public bool DFSToFuelSource(IGridMember gridCell, FuelType fuel, out List<PipeTemplate> path)
        {
            path = null;

            if (gridCell == null)
                return false;

            PipeTemplate pipeTemplate = gridCell as PipeTemplate;

            if (pipeTemplate.ConnectedTemplates.Count == 0 ||
                (pipeTemplate.FuelType != fuel && pipeTemplate.FuelType != FuelType.Any))
                return false;

            path = new List<PipeTemplate>();
            List<PipeTemplate> checkedTemplates = new List<PipeTemplate>();
            Stack<PipeTemplate> templatesToCheck = new Stack<PipeTemplate>();

            templatesToCheck.Push(pipeTemplate);
            path.Add(pipeTemplate);

            PipeTemplate checkingTemplate;

            do
            {
                checkingTemplate = templatesToCheck.Pop();
                path.Add(checkingTemplate);

                checkedTemplates.Add(checkingTemplate);

                foreach (PipeTemplate connectedTemplate in checkingTemplate.ConnectedTemplates)
                {
                    if (templatesToCheck.Contains(connectedTemplate) == false &&
                        checkedTemplates.Contains(connectedTemplate) == false &&
                        (connectedTemplate.FuelType == FuelType.Any || connectedTemplate.FuelType == fuel))
                    {
                        templatesToCheck.Push(connectedTemplate);

                        if (connectedTemplate == _grid.FuelSourcePoint)
                        {
                            checkingTemplate = templatesToCheck.Pop();
                            path.Add(checkingTemplate);
                            path = FormPath(pipeTemplate, templatesToCheck, path);
                            return true;
                        }
                    }
                }
            }
            while (templatesToCheck.Count > 0);

            path = null;

            return false;
        }

        private List<PipeTemplate> FormPath(
            PipeTemplate refuelingPoint, Stack<PipeTemplate> removedTemplates, List<PipeTemplate> path)
        {
            int removedInIteration;
            List<PipeTemplate> newPath = path.Except(removedTemplates).ToList();

            path = new List<PipeTemplate>(newPath);

            do
            {
                removedInIteration = 0;

                foreach (PipeTemplate template in path)
                {
                    if (template != _grid.FuelSourcePoint &&
                        template != refuelingPoint)
                    {
                        if (template.ConnectedTemplates.Except(removedTemplates).Count() == 1)
                        {
                            removedTemplates.Push(template);
                            newPath.Remove(template);
                            removedInIteration++;
                        }
                    }
                }

                path = new List<PipeTemplate>(newPath);
            }
            while (removedInIteration != 0);

            return path;
        }
    }
}