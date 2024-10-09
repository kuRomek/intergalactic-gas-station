public interface IGridMember
{
    int[] GridPosition { get; }

    void PlaceOnGrid(IGrid grid);
}
