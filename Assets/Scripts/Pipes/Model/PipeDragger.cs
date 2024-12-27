using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IntergalacticGasStation.Fuel;
using IntergalacticGasStation.Input;
using IntergalacticGasStation.StructureElements;
using Grid = IntergalacticGasStation.LevelGrid.Grid;

namespace IntergalacticGasStation
{
    namespace Pipes
    {
        public class PipeDragger : IActivatable
        {
            private PlayerInputController _input;
            private Grid _grid;
            private FuelProvider _fuelProvider;
            private PipeTemplate _draggingPipeTemplate = null;

            public PipeDragger(PlayerInputController playerInputController, Grid grid, FuelProvider fuelProvider)
            {
                _input = playerInputController;
                _grid = grid;
                _fuelProvider = fuelProvider;
            }

            public void Enable()
            {
                _input.DragStarted += OnDragStarted;
                _input.Dragging += OnDragging;
                _input.DragCanceled += OnDragCanceled;
            }

            public void Disable()
            {
                _input.DragStarted -= OnDragStarted;
                _input.Dragging -= OnDragging;
                _input.DragCanceled -= OnDragCanceled;
            }

            private void OnDragStarted(PipeTemplate pipeTemplate)
            {
                if (_draggingPipeTemplate == null &&
                    (_fuelProvider.Path == null || _fuelProvider.Path.Contains(pipeTemplate) == false))
                {
                    _draggingPipeTemplate = pipeTemplate;

                    _grid.Remove(pipeTemplate);

                    List<PipeTemplate> connectedTemplates = new List<PipeTemplate>(pipeTemplate.ConnectedTemplates);

                    foreach (PipeTemplate connectedTemplate in connectedTemplates)
                        pipeTemplate.Disconnect(connectedTemplate);

                    _draggingPipeTemplate.MoveTo(_draggingPipeTemplate.Position - Vector3.forward);

                    foreach (PipePiece pipe in _draggingPipeTemplate.PipePieces)
                        pipe.MoveTo(pipe.Position - Vector3.forward);
                }
            }

            private void OnDragging(Vector3 delta)
            {
                if (_draggingPipeTemplate != null)
                {
                    _draggingPipeTemplate.MoveTo(_draggingPipeTemplate.Position + delta);

                    foreach (PipePiece pipe in _draggingPipeTemplate.PipePieces)
                        pipe.MoveTo(pipe.Position + delta);
                }
            }

            private void OnDragCanceled()
            {
                if (_draggingPipeTemplate != null)
                {
                    _draggingPipeTemplate.MoveTo(_draggingPipeTemplate.Position + Vector3.forward);

                    foreach (PipePiece pipe in _draggingPipeTemplate.PipePieces)
                        pipe.MoveTo(pipe.Position + Vector3.forward);

                    _grid.Place(_draggingPipeTemplate);

                    _draggingPipeTemplate = null;
                }
            }
        }
    }
}
