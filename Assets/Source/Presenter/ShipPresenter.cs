using UnityEngine;

[RequireComponent(typeof(ShipView))]
public class ShipPresenter : Presenter, IActivatable
{
    public new Ship Model => base.Model as Ship;
    public new ShipView View => base.View as ShipView;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Offscreen _))
            Model.Destroy();
    }

    public void Enable()
    {
        foreach (ShipTank shipTank in Model.Tanks)
            shipTank.FuelAmountChanged += () => View.ChangeView(shipTank);

        View.ViewChangingStopped += Model.OnViewChangingStopped;
    }

    public void Disable()
    {
        foreach (ShipTank shipTank in Model.Tanks)
            shipTank.FuelAmountChanged -= () => View.ChangeView(shipTank);

        View.ViewChangingStopped -= Model.OnViewChangingStopped;
    }
}
