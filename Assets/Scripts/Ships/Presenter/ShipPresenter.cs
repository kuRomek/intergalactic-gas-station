using System;
using UnityEngine;
using IntergalacticGasStation.Misc;
using IntergalacticGasStation.StructureElements;
using IntergalacticGasStation.Tanks;

namespace IntergalacticGasStation
{
    namespace Ships
    {
        [RequireComponent(typeof(ShipView))]
        public class ShipPresenter : Presenter, IActivatable
        {
            private Action[] _tanksViewChangings;

            public new Ship Model => base.Model as Ship;

            public new ShipView View => base.View as ShipView;

            private void OnTriggerEnter(Collider other)
            {
                if (other.TryGetComponent(out Offscreen _))
                    Model.Destroy();
            }

            public void Enable()
            {
                _tanksViewChangings = new Action[Model.Tanks.Count];

                int j = 0;

                foreach (ShipTank shipTank in Model.Tanks)
                {
                    _tanksViewChangings[j] = () => View.ChangeFuelAmount(shipTank);
                    shipTank.FuelAmountChanged += _tanksViewChangings[j++];
                }

                View.ViewChangingStopped += Model.OnFuelProvidingStopped;
                Model.ArrivingAtStation += View.PlayArrivalSound;
                Model.ArrivingAtStation += View.PlayBurstingAnimation;
                Model.ArrivedAtRefuelingPoint += View.PlayIdleAnimation;
                Model.LeavedStation += View.PlayFlyAwaySound;
                Model.LeavedStation += View.PlayBurstingAnimation;
            }

            public void Disable()
            {
                int j = 0;

                foreach (ShipTank shipTank in Model.Tanks)
                    shipTank.FuelAmountChanged -= _tanksViewChangings[j++];

                View.ViewChangingStopped -= Model.OnFuelProvidingStopped;
                Model.ArrivingAtStation -= View.PlayArrivalSound;
                Model.ArrivingAtStation -= View.PlayBurstingAnimation;
                Model.ArrivedAtRefuelingPoint -= View.PlayIdleAnimation;
                Model.LeavedStation -= View.PlayFlyAwaySound;
                Model.LeavedStation -= View.PlayBurstingAnimation;
            }
        }
    }
}
