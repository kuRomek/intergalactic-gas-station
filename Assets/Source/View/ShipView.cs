using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipView : View
{
    [SerializeField] private FuelView _fuelViewPrefab;
    [SerializeField] private Canvas _fuelViewPlace;

    private FuelView[] _fuelViews = null;

    public event Action<ITank> ViewChangingStopped;

    private void OnEnable()
    {
        if (_fuelViews != null)
        {
            foreach (FuelView view in _fuelViews)
                view.ViewChangingStopped += OnViewChangingStopped;
        }
    }

    private void OnDisable()
    {
        if (_fuelViews != null)
        {
            foreach (FuelView view in _fuelViews)
                view.ViewChangingStopped -= OnViewChangingStopped;
        }
    }

    public void ChangeView(ITank tank)
    {
        _fuelViews.FirstOrDefault(view => view.Tank == tank).ChangeView();
    }

    private void OnViewChangingStopped(ITank tank)
    {
        ViewChangingStopped?.Invoke(tank);
    }

    public void CreateFuelViews(IReadOnlyList<ShipTank> shipTanks)
    {
        _fuelViews = new FuelView[shipTanks.Count];

        for (int i = 0; i < shipTanks.Count; i++)
        {
            _fuelViews[i] = Instantiate(_fuelViewPrefab);
            _fuelViews[i].Init(shipTanks[i]);
            _fuelViews[i].transform.SetParent(_fuelViewPlace.transform);
            _fuelViews[i].ViewChangingStopped += OnViewChangingStopped;
        }
    }
}
