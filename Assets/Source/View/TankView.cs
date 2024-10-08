using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class TankView : View
{
    [SerializeField] private FuelTypes _fuelTypes;

    private Color _color;

    public void Init(Fuel fuel)
    {
        _color = _fuelTypes.GetColorOf(fuel);
        GetComponent<Renderer>().material.color = _color;
    }
}
