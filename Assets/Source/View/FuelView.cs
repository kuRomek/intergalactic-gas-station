using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FuelView : View
{
    [SerializeField] private Image _fuelIndicatorFilled;
    [SerializeField] private Image _fuelIndicatorColor;
    [SerializeField] private FuelColors _fuelCollors;

    private ITank _tank;

    public void Init(ITank tank)
    {
        _tank = tank;

        float originalHeight = _fuelIndicatorColor.rectTransform.sizeDelta.y;

        _fuelIndicatorFilled.rectTransform.sizeDelta = new Vector2(_fuelIndicatorFilled.rectTransform.sizeDelta.x, 
            originalHeight * (_tank.Capacity / ITank.MaximumSize));

        _fuelIndicatorColor.rectTransform.sizeDelta = new Vector2(_fuelIndicatorColor.rectTransform.sizeDelta.x,
            originalHeight * (_tank.Capacity / ITank.MaximumSize));

        _fuelIndicatorFilled.color = Color.white;
        _fuelIndicatorColor.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color;
        _fuelIndicatorFilled.fillAmount = _tank.CurrentAmount / _tank.Capacity;

        _tank.FuelAmountChanged += StartChangingView;
    }

    private void OnEnable()
    {
        if (_tank != null)
            _tank.FuelAmountChanged += StartChangingView;
    }

    private void OnDisable()
    {
        _tank.FuelAmountChanged -= StartChangingView;
    }

    private void StartChangingView()
    {
        StartCoroutine(ChangeView());
    }

    private IEnumerator ChangeView()
    {
        float currentAmountView = _fuelIndicatorFilled.fillAmount;

        while (Mathf.Abs(_tank.CurrentAmount / _tank.Capacity - currentAmountView) < 0.01f == false)
        {
            currentAmountView = Mathf.Lerp(currentAmountView, _tank.CurrentAmount / _tank.Capacity, Time.deltaTime * 4f);

            _fuelIndicatorFilled.fillAmount = currentAmountView;
            yield return null;
        }

        _fuelIndicatorFilled.fillAmount = _tank.CurrentAmount / _tank.Capacity;

        _tank.OnViewChangingStopped();
    }
}
