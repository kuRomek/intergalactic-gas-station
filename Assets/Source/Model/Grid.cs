using System;
using System.Linq;
using UnityEngine;

public class Grid : IGrid
{
    private const float WorldOffset = 2f;
    private const int Size = 5;

    private IGridMember[,] _cells = new IGridMember[Size, Size];
    private PipeDivider[] _pipeDividers;

    public Grid(PipeDivider[] pipeDividers)
    {
        _pipeDividers = pipeDividers;
    }

    public event Action PipelineChanged;

    public IGridMember[,] Cells => _cells;
    public IGridMember[] RefuelingPoints => new IGridMember[3] { _cells[2, 0], _cells[0, 2], _cells[2, 4] };
    public IGridMember FuelSourcePoint => _cells[4, 2];

    public Vector3 CalculateWorldPosition(int[] gridPosition)
    {
        if (gridPosition == null)
            throw new ArgumentNullException(nameof(gridPosition));

        if (gridPosition.Length != 2)
            throw new ArgumentException("The grid is 2-dimensional.");

        if (gridPosition[0] < 0 || gridPosition[0] > 4)
            throw new ArgumentException($"The size of the grid is {Size}x{Size}, so x position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[0]}");

        if (gridPosition[1] < 0 || gridPosition[1] > 4)
            throw new ArgumentException($"The size of the grid is {Size}x{Size}, so y position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[1]}");

        return new Vector3(gridPosition[1] - WorldOffset, -(gridPosition[0] - WorldOffset));
    }

    public int[] CalculateGridPosition(Vector3 worldPosition)
    {
        int[] gridPosition = new int[2] { (int)(-Mathf.Round(worldPosition.y) + WorldOffset), (int)(Mathf.Round(worldPosition.x) + WorldOffset) };

        if (gridPosition[0] < 0 || gridPosition[0] > 4)
            throw new ArgumentException($"The size of the grid is {Size}x{Size}, so x position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[0]}");

        if (gridPosition[1] < 0 || gridPosition[1] > 4)
            throw new ArgumentException($"The size of the grid is {Size}x{Size}, so y position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[1]}");

        if (_cells[gridPosition[0], gridPosition[1]] != null)
            throw new InvalidOperationException($"The cell ({gridPosition[0]}, {gridPosition[1]}) is already taken.");

        return gridPosition;
    }

    public void Place(PipeTemplate pipeTemplate)
    {
        Vector3 templateOriginalPosition;

        try
        {
            templateOriginalPosition = CalculateWorldPosition(pipeTemplate.GridPosition);

            RemoveTemplate(pipeTemplate);
        }
        catch (Exception exception) when (exception is ArgumentNullException)
        {
            templateOriginalPosition = default;
        }

        try
        {
            pipeTemplate.PlaceOnGrid(this);
        }
        catch (Exception exception) when (exception is ArgumentException || exception is InvalidOperationException)
        {
            pipeTemplate.MoveTo(templateOriginalPosition);

            foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                pipePiece.MoveTo(templateOriginalPosition + pipePiece.LocalPosition);

            pipeTemplate.PlaceOnGrid(this);
        }

        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
            _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = pipeTemplate;

        ConnectNearbyTemplates(pipeTemplate);
        PipelineChanged?.Invoke();
    }

    public void CheckConnections(PipeTemplate pipeTemplate)
    {
        int[][] indexOffsets = new int[4][]
        {
            new int[2] { 1, 0 },
            new int[2] { 0, 1 },
            new int[2] { -1, 0 },
            new int[2] { 0, -1 }
        };

        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
        {
            bool[] connections = new bool[4] { false, false, false, false };

            foreach (int[] offset in indexOffsets)
            {
                IGridMember checkingCell;

                try
                {
                    checkingCell = _cells[pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1]];

                    if (checkingCell != null)
                    {
                        //TODO: сделать провверку на подходящее топливо (а в dfs убрать)
                        if (checkingCell != pipeTemplate && checkingCell is PipeTemplate nearbyTemplate)
                        {
                            if (pipePiece.FuelType == Fuel.Default || nearbyTemplate.FuelType == pipePiece.FuelType || nearbyTemplate.FuelType == Fuel.Default)
                            {
                                int connectionNumber = Array.IndexOf(indexOffsets, offset);
                                connections[connectionNumber] = true;

                                int[] cell1 = new int[2] { pipePiece.GridPosition[0], pipePiece.GridPosition[1] };
                                int[] cell2 = new int[2] { pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1] };

                                _pipeDividers.FirstOrDefault(divider => (divider.Connection[0].SequenceEqual(cell1) &&
                                                                        divider.Connection[1].SequenceEqual(cell2)) ||
                                                                        (divider.Connection[0].SequenceEqual(cell2) &&
                                                                        divider.Connection[1].SequenceEqual(cell1))).gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            connections[Array.IndexOf(indexOffsets, offset)] = true;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (pipePiece.GridPosition[0] + offset[0] == 2 && (pipePiece.GridPosition[1] + offset[1] == -1 || pipePiece.GridPosition[1] + offset[1] == Size))
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    else if (pipePiece.GridPosition[0] + offset[0] == -1 && pipePiece.GridPosition[1] + offset[1] == 2)
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    else if (pipePiece.GridPosition[0] + offset[0] == Size && pipePiece.GridPosition[1] + offset[1] == 2)
                        connections[Array.IndexOf(indexOffsets, offset)] = true;

                    continue;
                }
            }

            pipePiece.EstablishVisualConnection(connections);
        }
    }

    public void RemoveTemplate(PipeTemplate pipeTemplate)
    {
        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
            _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = null;

        int[][] indexOffsets = new int[4][]
        {
            new int[2] { 1, 0 },
            new int[2] { 0, 1 },
            new int[2] { -1, 0 },
            new int[2] { 0, -1 }
        };

        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
        {
            foreach (int[] offset in indexOffsets)
            {
                IGridMember checkingCell;

                try
                {
                    checkingCell = _cells[pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1]];

                    if (checkingCell != null)
                    {
                        int[] cell1 = new int[2] { pipePiece.GridPosition[0], pipePiece.GridPosition[1]};
                        int[] cell2 = new int[2] { pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1] };

                        _pipeDividers.FirstOrDefault(divider => (divider.Connection[0].SequenceEqual(cell1) &&
                                                                        divider.Connection[1].SequenceEqual(cell2)) ||
                                                                        (divider.Connection[0].SequenceEqual(cell2) &&
                                                                        divider.Connection[1].SequenceEqual(cell1))).gameObject.SetActive(false);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
        }
    }

    private void ConnectNearbyTemplates(PipeTemplate pipeTemplate)
    {
        int[][] indexOffsets = new int[4][]
        {
            new int[2] { 1, 0 },
            new int[2] { 0, 1 },
            new int[2] { -1, 0 },
            new int[2] { 0, -1 }
        };

        foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
        {
            bool[] connections = new bool[4] { false, false, false, false };

            foreach (int[] offset in indexOffsets)
            {
                IGridMember checkingCell;

                try
                {
                    checkingCell = _cells[pipePiece.GridPosition[0] + offset[0], pipePiece.GridPosition[1] + offset[1]];

                    if (checkingCell != null)
                    {
                        //TODO: сделать провверку на подходящее топливо (а в dfs убрать)
                        if (checkingCell != pipeTemplate && checkingCell is PipeTemplate nearbyTemplate)
                        {
                            pipeTemplate.Connect(nearbyTemplate);

                            if (pipePiece.FuelType == Fuel.Default || nearbyTemplate.FuelType == pipePiece.FuelType || nearbyTemplate.FuelType == Fuel.Default)
                            {
                                connections[Array.IndexOf(indexOffsets, offset)] = true;
                                CheckConnections(nearbyTemplate);
                            }
                        }
                        else
                        {
                            connections[Array.IndexOf(indexOffsets, offset)] = true;
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (pipePiece.GridPosition[0] + offset[0] == 2 && (pipePiece.GridPosition[1] + offset[1] == -1 || pipePiece.GridPosition[1] + offset[1] == Size))
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    else if (pipePiece.GridPosition[0] + offset[0] == -1 && pipePiece.GridPosition[1] + offset[1] == 2)
                        connections[Array.IndexOf(indexOffsets, offset)] = true;
                    else if (pipePiece.GridPosition[0] + offset[0] == Size && pipePiece.GridPosition[1] + offset[1] == 2)
                        connections[Array.IndexOf(indexOffsets, offset)] = true;

                    continue;
                }
            }

            pipePiece.EstablishVisualConnection(connections);
        }
    }
}