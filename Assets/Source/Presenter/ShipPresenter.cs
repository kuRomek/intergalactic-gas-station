using System;
using UnityEngine;

[RequireComponent(typeof(ShipView))]
public class ShipPresenter : Presenter, IActivatable
{
    public new Ship Model => base.Model as Ship;
    public new ShipView View => base.View as ShipView;

    private Action[] _tanksViewChangings;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Offscreen _))
            Model.Destroy();
    }

    public void Enable()
    {
        _tanksViewChangings = new Action[Model.Tanks.Count];

        //for (int i = 0; i < _tanksViewChangings.Length; i++)
        //{
        //    _tanksViewChangings[i] = () => View.ChangeView(Model.Tanks[i]);
        //    Model.Tanks[i].FuelAmountChanged += _tanksViewChangings[i];
        //}

        foreach (ShipTank shipTank in Model.Tanks)
            shipTank.FuelAmountChanged += () => View.ChangeView(shipTank);

        View.ViewChangingStopped += Model.OnViewChangingStopped;
        Model.ArrivingAtStation += View.PlayArrivalSound;
        Model.ArrivingAtStation += View.PlayBurstingAnimation;
        Model.StoppedAtRefuelingPoint += View.PlayIdleAnimation;
        Model.LeavedStation += View.PlayFlyAwaySound;
        Model.LeavedStation += View.PlayBurstingAnimation;
    }

    public void Disable()
    {
        //for (int i = 0; i < _tanksViewChangings.Length; i++)
        //    Model.Tanks[i].FuelAmountChanged -= _tanksViewChangings[i];

        foreach (ShipTank shipTank in Model.Tanks)
            shipTank.FuelAmountChanged += () => View.ChangeView(shipTank);

        View.ViewChangingStopped -= Model.OnViewChangingStopped;
        Model.ArrivingAtStation -= View.PlayArrivalSound;
        Model.ArrivingAtStation -= View.PlayBurstingAnimation;
        Model.StoppedAtRefuelingPoint -= View.PlayIdleAnimation;
        Model.LeavedStation -= View.PlayFlyAwaySound;
        Model.LeavedStation -= View.PlayBurstingAnimation;
    }
}
