using UnityEngine;

public class PipeDragger : Transformable, IActivatable
{
    private PlayerInputController _input;
    private Grid _grid;
    private PipeTemplate _draggingPipeTemplate = null;

    public PipeDragger(PlayerInputController playerInputController, Grid grid) : base(default, default) 
    {
        _input = playerInputController;
        _grid = grid;
    }

    public void Enable()
    {
        _input.ButtonPressed += OnDragStarted;
        _input.Dragging += OnDragging;
        _input.DragCanceled += OnDragCanceled;
    }

    public void Disable()
    {
        _input.ButtonPressed -= OnDragStarted;
        _input.Dragging -= OnDragging;
        _input.DragCanceled -= OnDragCanceled;
    }

    private void OnDragStarted(PipeTemplate pipeTemplate)
    {
        _draggingPipeTemplate = pipeTemplate;

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
