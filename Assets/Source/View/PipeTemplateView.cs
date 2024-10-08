using UnityEngine;

public class PipeTemplateView : View
{
    [SerializeField] private FuelTypes _fuelTypes;

    private Color _color;

    public void Init(Fuel fuel)
    {
        _color = _fuelTypes.GetColorOf(fuel);

        foreach (Renderer pieceRenderer in GetComponentsInChildren<Renderer>())
            pieceRenderer.material.color = _color;
    }
}
