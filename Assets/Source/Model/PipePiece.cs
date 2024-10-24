using System;
using UnityEngine;

public class PipePiece : Transformable, IGridMember
{
    public PipePiece(Vector3 worldPosition, Vector3 centerPiecePosition, Quaternion rotation, Fuel fuelType) : base(worldPosition, rotation) 
    {
        LocalPosition = worldPosition - centerPiecePosition;
        FuelType = fuelType;
    }

    public event Action<bool[]> ConnectionIsEstablished;
    public event Action OriginalViewRecovering;

    public void PlaceOnGrid(IGrid grid)
    {
        GridPosition = grid.CalculateGridPosition(Position);
    }

    public void EstablishVisualConnection(bool[] connections)
    {
        ConnectionIsEstablished?.Invoke(connections);
    }

    public void RecoverOriginalView()
    {
        OriginalViewRecovering?.Invoke();
    }

    public Fuel FuelType { get; private set; }
    public Vector3 LocalPosition { get; private set; }
    public int[] GridPosition { get; private set; }
}
