using System;
using System.Collections.Generic;
using System.Linq;

public class PipeTemplate : Transformable, IGridMember
{
    private PipePiece[] _pipePieces;
    private List<PipeTemplate> _connectedTemplates = new List<PipeTemplate>();

    public PipeTemplate(PipePiece[] pipePieces, Fuel fuelType) : base(pipePieces[pipePieces.Length / 2].Position, default)
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
    public Fuel FuelType { get; }
    public int[] GridPosition { get; private set; } = null;

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
