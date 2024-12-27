namespace IntergalacticGasStation
{
    namespace LevelGrid
    {
        public interface IGridMember
        {
            int[] GridPosition { get; }

            void PlaceOnGrid(IGrid grid);
        }
    }
}
