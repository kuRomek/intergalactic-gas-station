using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TankView : View
{
    [SerializeField] private FuelColors _fuelTypes;
    [SerializeField] private FuelView _fuelView;

    private Color _color;

    public void Init(ITank tank)
    {
        _color = _fuelTypes.GetMaterialOf(tank.FuelType).color;
        GetComponent<Renderer>().material.color = _color;

        _fuelView.Init(tank);
        _fuelView.transform.localScale = new Vector3(_fuelView.transform.localScale.x, _fuelView.transform.localScale.y * ITank.MaximumSize / tank.Capacity, _fuelView.transform.localScale.z);
    }
}
