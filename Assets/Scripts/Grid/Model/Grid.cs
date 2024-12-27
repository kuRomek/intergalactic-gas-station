using System;
using System.Collections.Generic;
using UnityEngine;
using IntergalacticGasStation.Misc;
using IntergalacticGasStation.Pipes;

namespace IntergalacticGasStation
{
    namespace LevelGrid
    {
        public class Grid : IGrid
        {
            private const float WorldOffset = 2f;

            private IGridMember[,] _cells;
            private PipeConnector _pipeConnector;

            public Grid(PipeDivider[] pipeDividers)
            {
                _cells = new IGridMember[Size, Size];
                _pipeConnector = new PipeConnector(pipeDividers, this);
            }

            public event Action PipelineChanged;

            public int Size { get; } = 5;

            public IGridMember[,] Cells => _cells;

            public IGridMember[] RefuelingPoints => 
                new IGridMember[3] { _cells[Size / 2, 0], _cells[0, Size / 2], _cells[Size / 2, Size - 1] };

            public IGridMember FuelSourcePoint => _cells[Size - 1, Size / 2];

            public Vector3 CalculateWorldPosition(int[] gridPosition)
            {
                if (gridPosition == null)
                    throw new ArgumentNullException(nameof(gridPosition));

                if (gridPosition.Length != 2)
                    throw new ArgumentException("The grid is 2-dimensional.");

                if (gridPosition[0] < 0 || gridPosition[0] >= Size)
                {
                    throw new ArgumentException($"The size of the grid is {Size}x{Size}, " +
                        $"so x position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[0]}");
                }

                if (gridPosition[1] < 0 || gridPosition[1] >= Size)
                {
                    throw new ArgumentException($"The size of the grid is {Size}x{Size}, " +
                        $"so y position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[1]}");
                }

                return new Vector3(gridPosition[1] - WorldOffset, -(gridPosition[0] - WorldOffset));
            }

            public int[] CalculateGridPosition(Vector3 worldPosition)
            {
                int[] gridPosition = new int[2]
                {
                    (int)(-Mathf.Round(worldPosition.y) + WorldOffset),
                    (int)(Mathf.Round(worldPosition.x) + WorldOffset),
                };

                if (gridPosition[0] < 0 || gridPosition[0] >= Size)
                {
                    throw new ArgumentException($"The size of the grid is {Size}x{Size}, " +
                        $"so x position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[0]}");
                }

                if (gridPosition[1] < 0 || gridPosition[1] >= Size)
                {
                    throw new ArgumentException($"The size of the grid is {Size}x{Size}, " +
                        $"so y position must be in set {{0, ..., {Size - 1}}}, yours is {gridPosition[1]}");
                }

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
                    Remove(pipeTemplate);
                }
                catch (Exception exception) when (exception is ArgumentNullException)
                {
                    templateOriginalPosition = default;
                }

                if (pipeTemplate.IsActive == false)
                {
                    pipeTemplate.MoveTo(templateOriginalPosition);

                    foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                        pipePiece.MoveTo(templateOriginalPosition + pipePiece.LocalPosition);

                    pipeTemplate.PlaceOnGrid(this);

                    foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                        _cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = pipeTemplate;

                    return;
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

                _pipeConnector.ConnectNearbyTemplates(pipeTemplate);
                PipelineChanged?.Invoke();
            }

            public void Remove(PipeTemplate pipeTemplate)
            {
                foreach (PipePiece pipePiece in pipeTemplate.PipePieces)
                    Cells[pipePiece.GridPosition[0], pipePiece.GridPosition[1]] = null;

                pipeTemplate.RemoveFromGrid();

                List<PipeTemplate> connectedTemplates = new List<PipeTemplate>(pipeTemplate.ConnectedTemplates);

                foreach (PipeTemplate connectedTemplate in connectedTemplates)
                {
                    connectedTemplate.Disconnect(pipeTemplate);
                    _pipeConnector.ConnectNearbyTemplates(connectedTemplate, false);
                }
            }
        }
    }
}