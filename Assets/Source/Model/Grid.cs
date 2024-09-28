using System;
using UnityEngine;

public class Grid : Transformable
{
    private const float GridOffset = 2f;
    private const int GridSize = 5;

    private Fuel[,] _cells = new Fuel[GridSize, GridSize];

    public Grid(Vector3 position, Quaternion rotation) : base(position, rotation) { }

    public event Action PipelineChanged;

    public Fuel[,] Cells => _cells;
    public int[] FuelSourcePoint => new int[2] { 4, 2 };
    public int[] LeftRefuelingPoint => new int[2] { 2, 0 };
    public int[] TopRefuelingPoint => new int[2] { 0, 2 };
    public int[] RightRefuelingPoint => new int[2] { 2, 4 };

    public static Vector3 CalculateWorldPosition(int[] gridPosition)
    {
        if (gridPosition.Length != 2)
            throw new ArgumentException("The grid is 2-dimensional.");

        if (gridPosition[0] < 0 || gridPosition[0] > 4)
            throw new ArgumentException($"The size of the grid is {GridSize}x{GridSize}, so x position must be in set {{0, ..., {GridSize - 1}}}, yours is {gridPosition[0]}");

        if (gridPosition[1] < 0 || gridPosition[1] > 4)
            throw new ArgumentException($"The size of the grid is {GridSize}x{GridSize}, so y position must be in set {{0, ..., {GridSize - 1}}}, yours is {gridPosition[1]}");

        return new Vector3(gridPosition[1] - GridOffset, -(gridPosition[0] - GridOffset));
    }

    public int[] CalculateGridPosition(Vector3 worldPosition)
    {
        int[] gridPosition = new int[2] { (int)(-Mathf.Round(worldPosition.y) + GridOffset), (int)(Mathf.Round(worldPosition.x) + GridOffset) };

        if (gridPosition[0] < 0 || gridPosition[0] > 4)
            throw new ArgumentException($"The size of the grid is {GridSize}x{GridSize}, so x position must be in set {{0, ..., {GridSize - 1}}}, yours is {gridPosition[0]}");

        if (gridPosition[1] < 0 || gridPosition[1] > 4)
            throw new ArgumentException($"The size of the grid is {GridSize}x{GridSize}, so y position must be in set {{0, ..., {GridSize - 1}}}, yours is {gridPosition[1]}");

        if (_cells[gridPosition[0], gridPosition[1]] != Fuel.Empty)
            throw new InvalidOperationException($"The cell ({gridPosition[0]}, {gridPosition[1]}) is already taken.");

        return gridPosition;
    }

    public void Place(PipeTemplate pipeTemplate)
    {
        Fuel[,] originalCells = (Fuel[,])_cells.Clone();

        int[] newTemplatePositionOnGrid = (int[])pipeTemplate.GridPosition.Clone();
        int[] oldTemplatePositionOnGrid = (int[])pipeTemplate.GridPosition.Clone();

        foreach (PipePiece pipe in pipeTemplate.PipePieces)
            _cells[pipe.GridPosition[0], pipe.GridPosition[1]] = Fuel.Empty;

        foreach (PipePiece pipe in pipeTemplate.PipePieces)
        {
            try
            {
                int[] gridCoordinates = CalculateGridPosition(pipe.Position);

                if (pipe == pipeTemplate.CenterPiece)
                    newTemplatePositionOnGrid = gridCoordinates;
                
                _cells[gridCoordinates[0], gridCoordinates[1]] = pipe.FuelType;
            }
            catch (Exception exception)
            {
                _cells = originalCells;

                newTemplatePositionOnGrid = oldTemplatePositionOnGrid;

                WriteException(exception.Message);
                break;
            }
        }

        pipeTemplate.FixOnGrid(newTemplatePositionOnGrid);
        PipelineChanged?.Invoke();
    }
}
