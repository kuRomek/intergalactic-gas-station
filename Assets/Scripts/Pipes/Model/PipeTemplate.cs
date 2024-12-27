using System;
using System.Collections.Generic;
using System.Linq;
using IntergalacticGasStation.Fuel;
using IntergalacticGasStation.LevelGrid;
using IntergalacticGasStation.StructureElements;

namespace IntergalacticGasStation
{
    namespace Pipes
    {
        public class PipeTemplate : Transformable, IGridMember, IActivatable
        {
            private PipePiece[] _pipePieces;
            private List<PipeTemplate> _connectedTemplates = new List<PipeTemplate>();

            public PipeTemplate(PipePiece[] pipePieces, FuelType fuelType)
                : base(pipePieces[pipePieces.Length / 2].Position, default)
            {
                _pipePieces = pipePieces ?? throw new ArgumentNullException(nameof(pipePieces));
                CenterPiece = pipePieces[pipePieces.Length / 2];

                FuelType = fuelType;
            }

            public event Action PlacedOnGrid;

            public event Action Providing;

            public event Action ProvidingStopped;

            public IReadOnlyCollection<PipePiece> PipePieces => _pipePieces;

            public IReadOnlyList<PipeTemplate> ConnectedTemplates => _connectedTemplates;

            public PipePiece CenterPiece { get; private set; }

            public FuelType FuelType { get; }

            public int[] GridPosition { get; private set; } = null;

            public bool IsActive { get; private set; } = false;

            public void Enable()
            {
                IsActive = true;
            }

            public void Disable()
            {
                IsActive = false;
            }

            public void ProvideFuel()
            {
                Providing?.Invoke();
            }

            public void StopProvidingFuel()
            {
                ProvidingStopped?.Invoke();
            }

            public void PlaceOnGrid(IGrid grid)
            {
                foreach (PipePiece pipePiece in _pipePieces)
                    pipePiece.PlaceOnGrid(grid);

                GridPosition = CenterPiece.GridPosition;

                MoveTo(grid.CalculateWorldPosition(GridPosition));

                foreach (PipePiece pipePiece in _pipePieces)
                    pipePiece.MoveTo(grid.CalculateWorldPosition(pipePiece.GridPosition));

                PlacedOnGrid?.Invoke();
            }

            public void RemoveFromGrid()
            {
                foreach (PipePiece pipePiece in _pipePieces)
                    pipePiece.RemoveFromGrid();
            }

            public void Connect(PipeTemplate pipeTemplate)
            {
                if (pipeTemplate == null)
                    throw new ArgumentNullException(nameof(pipeTemplate));

                if (ConnectedTemplates.Contains(pipeTemplate) == false)
                {
                    _connectedTemplates.Add(pipeTemplate);
                    pipeTemplate.Connect(this);
                }
            }

            public void Disconnect(PipeTemplate pipeTemplate)
            {
                if (pipeTemplate == null)
                    throw new ArgumentNullException(nameof(pipeTemplate));

                if (_connectedTemplates.Remove(pipeTemplate) == true)
                    pipeTemplate.Disconnect(this);
            }
        }
    }
}
