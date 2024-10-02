using System;
using UnityEngine;

public class Grid : Transformable, IGrid
{
    private const float WorldOffset = 2f;
    private const int Size = 5;

    private IGridMember[,] _cells = new IGridMember[Size, Size];

    public Grid(Vector3 position, Quaternion rotation) : base(position, rotation) { }

    public IGridMember[,] Cells => _cells;
    public int[] FuelSourcePoint => new int[2] { 4, 2 };
    public int[][] RefuelingPoints => new int[3][]
    {
        new int[2] { 2, 0 },
        new int[2] { 0, 2 },
        new int[2] { 2, 4 } 
    };

    public Vector3 CalculateWorldPosition(int[] gridPosition)
    {
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

            foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = null;
        }
        catch (NullReferenceException)
        {
            templateOriginalPosition = default;
        }

        try
        {
            pipeTemplate.PlaceOnGrid(this);

            foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = pipeTemplate;
        }
        catch (Exception exception)
        {
            pipeTemplate.MoveTo(templateOriginalPosition);

            foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
            {
                pipePiece.MoveTo(templateOriginalPosition + pipePiece.LocalPosition);
                _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = null;
            }

            pipeTemplate.PlaceOnGrid(this);
            WriteException(exception.Message);
        }
    }
}
