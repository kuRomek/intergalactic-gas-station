public class PipePiece : Transformable
{
    public PipePiece(int[] positionOnGrid) : base(Grid.CalculateWorldPosition(positionOnGrid), default) 
    {
        GridPosition = positionOnGrid;
    }

    public Fuel FuelType { get; private set; } = Fuel.Default;
    public int[] GridPosition { get; private set; } = new int[2];

    public void ShiftOnGrid(int[] shift)
    {
        GridPosition[0] += shift[0];
        GridPosition[1] += shift[1];

        MoveTo(Grid.CalculateWorldPosition(GridPosition));
    }
}
