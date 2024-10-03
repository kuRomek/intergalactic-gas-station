using UnityEngine;

public class PipePiece : Transformable, IGridMember
{
    public PipePiece(Vector3 worldPosition, Vector3 centerPiecePosition) : base(worldPosition, default) 
    {
        LocalPosition = worldPosition - centerPiecePosition;
    }

    public Fuel FuelType { get; private set; } = Fuel.Default;
    public Vector3 LocalPosition { get; private set; }
    public int[] GridPosition { get; private set; }

    public void PlaceOnGrid(IGrid grid)
    {
        GridPosition = grid.CalculateGridPosition(Position);
    }
}
