using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using IntergalacticGasStation.StructureElements;
using IntergalacticGasStation.Tanks;

namespace IntergalacticGasStation
{
    namespace Fuel
    {
        public class FuelView : View
        {
            [SerializeField] private Slider _fuelIndicator;
            [SerializeField] private Image _backgroundImage;
            [SerializeField] private FuelColors _fuelCollors;
            [SerializeField] private AudioClip _refuelingSound;

            private ITank _tank;
            private Coroutine _changingView;

            public event Action<ITank> ViewChangingStopped;

            public ITank Tank => _tank;

            public void Init(ITank tank)
            {
                _tank = tank;

                _fuelIndicator.minValue = 0;
                _fuelIndicator.maxValue = _tank.Capacity;
                _fuelIndicator.value = _tank.CurrentAmount;

                if (_tank is ShipTank)
                    _backgroundImage.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color * 2f;
                else
                    _backgroundImage.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color;

                RectTransform sliderRect = _fuelIndicator.GetComponent<RectTransform>();

                float originalHeight = sliderRect.sizeDelta.y;

                sliderRect.sizeDelta = new Vector2(sliderRect.sizeDelta.x, originalHeight * (_tank.Capacity / ITank.MaximumSize));
            }

            public void ChangeAmount()
            {
                if (_changingView != null)
                    StopCoroutine(_changingView);

                _changingView = StartCoroutine(StartChangingView());
            }

            private IEnumerator StartChangingView()
            {
                float currentAmountView = _fuelIndicator.value;

                if (_refuelingSound != null)
                    PlaySound(_refuelingSound);

                while (Mathf.Abs(_tank.CurrentAmount - currentAmountView) < 0.05f == false)
                {
                    currentAmountView = Mathf.Lerp(currentAmountView, _tank.CurrentAmount, Time.deltaTime * 4f);

                    _fuelIndicator.value = currentAmountView;
                    yield return null;
                }

                _fuelIndicator.value = _tank.CurrentAmount;

                ViewChangingStopped?.Invoke(_tank);
            }
        }
    }
}