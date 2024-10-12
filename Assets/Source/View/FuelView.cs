using System;
using TMPro;
using UnityEngine;

public class FuelView : View
{
    [SerializeField] private TextMeshProUGUI _currentAmount;
    [SerializeField] private TextMeshProUGUI _maxAmount;
    [SerializeField] private FuelColors _fuelCollors;

    private ITank _tank;

    public void Init(ITank tank)
    {
        _tank = tank;
        _maxAmount.text = Convert.ToString(_tank.Capacity);

        _maxAmount.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color;
        _currentAmount.color = _fuelCollors.GetMaterialOf(_tank.FuelType).color;

        OnFuelAmountChanged();

        _tank.FuelAmountChanged += OnFuelAmountChanged;
    }

    private void OnDisable()
    {
        _tank.FuelAmountChanged -= OnFuelAmountChanged;
    }

    private void OnFuelAmountChanged()
    {
        _currentAmount.text = Convert.ToString(_tank.CurrentAmount);
    }
}
