using System;
using UnityEngine;
using Fuel;
using StructureElements;

namespace Tanks
{
    public class TankView : View
    {
        [SerializeField] private FuelColors _fuelTypes;
        [SerializeField] private FuelView _fuelView;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Color _color;

        public event Action ViewChangingStopped;

        private void OnEnable()
        {
            _fuelView.ViewChangingStopped += OnViewChangingStopped;
        }

        private void OnDisable()
        {
            _fuelView.ViewChangingStopped -= OnViewChangingStopped;
        }

        public void CreateFuelView(ITank tank)
        {
            _color = _fuelTypes.GetMaterialOf(tank.FuelType).color;
            _meshRenderer.material.color = _color;

            _meshRenderer.transform.localScale = new Vector3(1f, tank.Capacity / (int)Size.Big, 1f);

            _fuelView.Init(tank);
        }

        public void ChangeView()
        {
            _fuelView.ChangeAmount();
        }

        private void OnViewChangingStopped(ITank tank)
        {
            ViewChangingStopped?.Invoke();
        }
    }
}
