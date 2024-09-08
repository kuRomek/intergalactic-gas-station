public class ShipTank
{
    public ShipTank(Fuel fuel)
    {
        Fuel = fuel;
    }

    public Fuel Fuel { get; }
    public bool IsFull { get; private set; } = false;

    public void Refuel()
    {
        if (IsFull)
            throw new System.InvalidOperationException("Tank is already full.");

        IsFull = true;
    }
}
