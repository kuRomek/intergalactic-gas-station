public interface ITank
{
    const int MaximumSize = 6;

    enum Size
    {
        Big = MaximumSize,
        Medium = 3,
        Small = 2
    }

    public Fuel FuelType { get; }
    public int Capacity { get; }
}