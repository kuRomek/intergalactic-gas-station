using System;
using UnityEngine;

public class PipePiece : Transformable, IGridMember
{
    public PipePiece(Vector3 worldPosition, Vector3 centerPiecePosition, Quaternion rotation, Fuel fuelType) : base(worldPosition, rotation) 
    {
        LocalPosition = worldPosition - centerPiecePosition;
        FuelType = fuelType;
    }

    public event Action<bool[]> ConnectionIsEstablishing;
    public event Action OriginalViewRecovering;

    public void PlaceOnGrid(IGrid grid)
    {
        GridPosition = grid.CalculateGridPosition(Position);
    }

    public Fuel FuelType { get; private set; }
    public Vector3 LocalPosition { get; private set; }
    public int[] GridPosition { get; private set; }

    public void EstablishVisualConnection(bool[] connections)
    {
        ConnectionIsEstablishing?.Invoke(connections);
    }

    public void RecoverOriginalView()
    {
        OriginalViewRecovering?.Invoke();
    }
}
