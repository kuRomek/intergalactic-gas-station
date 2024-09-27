using System;
using System.Collections.Generic;

public class PipeTemplate : Transformable
{
    private PipePiece[] _pipePieces;

    public IReadOnlyCollection<PipePiece> PipePieces => _pipePieces;
    public int[] GridPosition => CenterPiece.GridPosition;
    public PipePiece CenterPiece { get; private set; }

    public PipeTemplate(PipePiece[] pipePieces) : base(pipePieces[pipePieces.Length / 2].Position, default)
    {
        _pipePieces = pipePieces ?? throw new ArgumentNullException(nameof(pipePieces));
        CenterPiece = pipePieces[pipePieces.Length / 2];
    }

    public void FixOnGrid(int[] gridPosition)
    {
        int[] shift = { gridPosition[0] - GridPosition[0], gridPosition[1] - GridPosition[1] };
        
        MoveTo(Grid.CalculateWorldPosition(gridPosition));

        foreach (PipePiece pipe in _pipePieces)
            pipe.ShiftOnGrid(shift);
    }
}
