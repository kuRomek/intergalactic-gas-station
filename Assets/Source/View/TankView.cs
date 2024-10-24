using UnityEngine;

public class TankView : View
{
    [SerializeField] private FuelColors _fuelTypes;
    [SerializeField] private FuelView _fuelView;
    [SerializeField] private MeshRenderer _meshRenderer;

    private Color _color;
    private ITank _tank;

    public void Init(ITank tank)
    {
        _tank = tank;

        _color = _fuelTypes.GetMaterialOf(_tank.FuelType).color;
        _meshRenderer.material.color = _color;

        _meshRenderer.transform.localScale = new Vector3(1f, _tank.Capacity / ITank.MaximumSize, 1f);

        _fuelView.Init(_tank);
    }
}
