using System;
using UnityEngine;
using Fuel;
using LevelGrid;
using StructureElements;

namespace Pipes
{
    public class PipePiece : Transformable, IGridMember
    {
        public PipePiece(Vector3 worldPosition, Vector3 centerPiecePosition, Quaternion rotation, FuelType fuelType)
            : base(worldPosition, rotation)
        {
            LocalPosition = worldPosition - centerPiecePosition;
            FuelType = fuelType;
        }

        public event Action<bool[]> ConnectionIsEstablishing;

        public event Action RemovedFromGrid;

        public FuelType FuelType { get; private set; }

        public Vector3 LocalPosition { get; private set; }

        public int[] GridPosition { get; private set; }

        public void PlaceOnGrid(IGrid grid)
        {
            GridPosition = grid.CalculateGridPosition(Position);
        }

        public void EstablishConnection(bool[] connections)
        {
            ConnectionIsEstablishing?.Invoke(connections);
        }

        public void RemoveFromGrid()
        {
            RemovedFromGrid?.Invoke();
        }
    }
}
