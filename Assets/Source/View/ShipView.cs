using UnityEngine;

public class ShipView : View
{
    [SerializeField] private FuelView _fuelViewPrefab;
    [SerializeField] private Canvas _fuelViewPlace;

    public void Init(ShipTank[] shipTanks)
    {
        foreach (ShipTank shipTank in shipTanks)
        {
            FuelView fuelView = Instantiate(_fuelViewPrefab);
            fuelView.Init(shipTank);
            fuelView.transform.SetParent(_fuelViewPlace.transform);
        }
    }
}
