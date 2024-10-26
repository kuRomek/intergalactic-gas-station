using System.Collections.Generic;
using UnityEngine;

public class PipeDragger : IActivatable
{
    private PlayerInputController _input;
    private Grid _grid;
    private PipeTemplate _draggingPipeTemplate = null;

    public PipeDragger(PlayerInputController playerInputController, Grid grid) 
    {
        _input = playerInputController;
        _grid = grid;
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
        _draggingPipeTemplate = pipeTemplate;

        _grid.RemoveTemplate(pipeTemplate);
        pipeTemplate.RecoverOriginalView();

        List<PipeTemplate> connectedTemplates = new List<PipeTemplate>();

        connectedTemplates.AddRange(pipeTemplate.ConnectedTemplates);

        foreach (PipeTemplate connectedTemplate in connectedTemplates)
        {
            pipeTemplate.Disconnect(connectedTemplate);
            _grid.CheckConnections(connectedTemplate);
        }

        _draggingPipeTemplate.MoveTo(_draggingPipeTemplate.Position - Vector3.forward);

        foreach (PipePiece pipe in _draggingPipeTemplate.PipePieces)
            pipe.MoveTo(pipe.Position - Vector3.forward);
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
