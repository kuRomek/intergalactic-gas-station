using UnityEngine;

[RequireComponent(typeof(TankView))]
public class TankPresenter : Presenter, IActivatable
{
    public new Tank Model => base.Model as Tank;

    public new TankView View => base.View as TankView;

    public void Enable()
    {
        Model.FuelAmountChanged += View.ChangeView;
        View.ViewChangingStopped += Model.OnFuelProvidingStopped;
    }

    public void Disable()
    {
        Model.FuelAmountChanged -= View.ChangeView;
        View.ViewChangingStopped -= Model.OnFuelProvidingStopped;
    }
}
