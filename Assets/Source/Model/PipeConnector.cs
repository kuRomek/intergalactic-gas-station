using System;
using System.Linq;

public class PipeConnector
{
    private IGrid _grid;
    private PipeDivider[] _pipeDividers;

    public PipeConnector(PipeDivider[] pipeDividers, IGrid grid)
    {
        _grid = grid;
        _pipeDividers = pipeDividers;
    }

    public void ConnectNearbyTemplates(PipeTemplate pipeTemplate, bool recursive = true)
    {
        int[][] indexOffsets = new int[4][]
        {
            new int[2] { 1, 0 },
            new int[2] { 0, 1 },
            new int[2] { -1, 0 },
            new int[2] { 0, -1 },
        };

        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
        {
            bool[] connections = new bool[4] { false, false, false, false };

            foreach (int[] offset in indexOffsets)
            {
                IGridMember checkingCell;

                try
                {
                    checkingCell = _grid.Cells[pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1]];

                    int[] cell1 = new int[2] { pipePiece.GridPosition[0], pipePiece.GridPosition[1] };
                    int[] cell2 = new int[2] { pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1] };

                    PipeDivider pipeDivider = _pipeDividers.FirstOrDefault(divider =>
                                                            (divider.Connection[0].SequenceEqual(cell1) &&
                                                            divider.Connection[1].SequenceEqual(cell2)) ||
                                                            (divider.Connection[0].SequenceEqual(cell2) &&
                                                            divider.Connection[1].SequenceEqual(cell1)));

                    if (checkingCell != null)
                    {
                        if (checkingCell != pipeTemplate && checkingCell is PipeTemplate nearbyTemplate)
                        {
                            if (pipeTemplate.FuelType == Fuel.Any || nearbyTemplate.FuelType == Fuel.Any ||
                                nearbyTemplate.FuelType == pipeTemplate.FuelType)
                            {
                                pipeTemplate.Connect(nearbyTemplate);
                                connections[Array.IndexOf(indexOffsets, offset)] = true;
                                pipeDivider.gameObject.SetActive(true);

                                if (recursive)
                                    ConnectNearbyTemplates(nearbyTemplate, false);
                            }
                            else
                            {
                                connections[Array.IndexOf(indexOffsets, offset)] = false;
                            }
                        }
                        else
                        {
                            connections[Array.IndexOf(indexOffsets, offset)] = true;
                            pipeDivider.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        pipeDivider.gameObject.SetActive(false);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (pipePiece.GridPosition[0] + offset[0] == 2 &&
                        (pipePiece.GridPosition[1] + offset[1] == -1 || pipePiece.GridPosition[1] + offset[1] == _grid.Size))
                    {
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    }
                    else if (pipePiece.GridPosition[0] + offset[0] == -1 && pipePiece.GridPosition[1] + offset[1] == 2)
                    {
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    }
                    else if (pipePiece.GridPosition[0] + offset[0] == _grid.Size && pipePiece.GridPosition[1] + offset[1] == 2)
                    {
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    }

                    continue;
                }
            }

            pipePiece.EstablishConnection(connections);
        }
    }
}