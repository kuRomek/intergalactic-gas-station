using System;
using TMPro;
using UnityEngine;

public class FuelView : View
{
    [SerializeField] private TextMeshProUGUI _currentAmount;
    [SerializeField] private TextMeshProUGUI _maxAmount;
    [SerializeField] private FuelCollors _fuelCollors;

    private ShipTank _tank;

    public void Init(ShipTank tank)
    {
        _tank = tank;
        _maxAmount.text = Convert.ToString(_tank.Capacity);

        _maxAmount.color = _fuelCollors.GetColorOf(_tank.FuelType);
        _currentAmount.color = _fuelCollors.GetColorOf(_tank.FuelType);

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
