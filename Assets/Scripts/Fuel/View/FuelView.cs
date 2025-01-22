using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using StructureElements;
using Tanks;

namespace Fuel
{
    public class FuelView : View
    {
        private const float AmountTolerance = 0.05f;

        [SerializeField] private Slider _fuelIndicator;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private FuelColors _fuelCollors;
        [SerializeField] private AudioClip _refuelingSound;

        private ITank _tank;
        private Coroutine _changingView;
        private float _changingSpeed = 4f;
        private float _shipTankColorMultiplier = 2f;

        public event Action<ITank> ViewChangingStopped;

        public ITank Tank => _tank;

        public void Init(ITank tank)
        {
            _tank = tank;

            _fuelIndicator.minValue = 0;
            _fuelIndicator.maxValue = _tank.Capacity;
            _fuelIndicator.value = _tank.CurrentAmount;

            if (_tank is ShipTank)
                _backgroundImage.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color * _shipTankColorMultiplier;
            else
                _backgroundImage.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color;

            RectTransform sliderRect = _fuelIndicator.GetComponent<RectTransform>();

            float originalHeight = sliderRect.sizeDelta.y;

            sliderRect.sizeDelta =
                new Vector2(sliderRect.sizeDelta.x, originalHeight * (_tank.Capacity / (int)Size.Big));
        }

        public void ChangeAmount()
        {
            if (_changingView != null)
                StopCoroutine(_changingView);

            _changingView = StartCoroutine(StartChangingView());
        }

        private IEnumerator StartChangingView()
        {
            float currentIndicatorAmount = _fuelIndicator.value;

            if (_refuelingSound != null)
                PlaySound(_refuelingSound);

            while (Mathf.Abs(_tank.CurrentAmount - currentIndicatorAmount) < AmountTolerance == false)
            {
                currentIndicatorAmount =
                    Mathf.Lerp(currentIndicatorAmount, _tank.CurrentAmount, Time.deltaTime * _changingSpeed);

                _fuelIndicator.value = currentIndicatorAmount;
                yield return null;
            }

            _fuelIndicator.value = _tank.CurrentAmount;

            ViewChangingStopped?.Invoke(_tank);
        }
    }
}