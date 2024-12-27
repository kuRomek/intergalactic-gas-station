using UnityEngine;
using TMPro;
using IntergalacticGasStation.LevelControl;

namespace IntergalacticGasStation
{
    namespace UI
    {
        public class ShipCounter : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _refueled;
            [SerializeField] private TextMeshProUGUI _needToRefuel;

            private LevelState _levelState;

            private void OnEnable()
            {
                _levelState.ShipRefueled += OnShipRefueled;
            }

            private void OnDisable()
            {
                _levelState.ShipRefueled -= OnShipRefueled;
            }

            public void Init(LevelState levelState)
            {
                _levelState = levelState;

                if (_needToRefuel != null)
                    _needToRefuel.text = _levelState.ShipCountOnLevel.ToString();

                _refueled.text = "0";

                enabled = true;
            }

            private void OnShipRefueled()
            {
                _refueled.text = _levelState.RefueledShipCount.ToString();
            }
        }
    }
}
