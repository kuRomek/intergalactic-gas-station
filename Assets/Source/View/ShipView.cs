using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipView : View
{
    [SerializeField] private FuelView _fuelViewPrefab;
    [SerializeField] private Transform _tanksViewPlace;
    [SerializeField] private Canvas _fuelViewPlace;
    [SerializeField] private Animator _burstAnimator;
    [SerializeField] private AudioClip _flyAwaySound;
    [SerializeField] private AudioClip _arrivalSound;

    public readonly int IsBursting = Animator.StringToHash(nameof(IsBursting));
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

    public void PlayFlyAwaySound(Ship _)
    {
        PlaySound(_flyAwaySound);
    }

    public void PlayArrivalSound(Ship _)
    {
        PlaySound(_arrivalSound);
    }

    public void PlayBurstingAnimation(Ship _)
    {
        _burstAnimator.SetBool(IsBursting, true);
        _tanksViewPlace.gameObject.SetActive(false);
    }

    public void PlayIdleAnimation()
    {
        _burstAnimator.SetBool(IsBursting, false);
        _tanksViewPlace.gameObject.SetActive(true);
    }

    private void OnViewChangingStopped(ITank tank)
    {
        ViewChangingStopped?.Invoke(tank);
    }
}
