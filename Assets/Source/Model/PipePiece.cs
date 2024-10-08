using UnityEngine;

public class PipePiece : Transformable, IGridMember
{
    public PipePiece(Vector3 worldPosition, Vector3 centerPiecePosition, Fuel fuelType) : base(worldPosition, default) 
    {
        LocalPosition = worldPosition - centerPiecePosition;
        FuelType = fuelType;
    }

    public Fuel FuelType { get; private set; }
    public Vector3 LocalPosition { get; private set; }
    public int[] GridPosition { get; private set; }

    public void PlaceOnGrid(IGrid grid)
    {
        GridPosition = grid.CalculateGridPosition(Position);
    }
}
