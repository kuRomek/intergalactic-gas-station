using UnityEngine;

public class PipeTemplateView : View
{
    [SerializeField] private FuelColors _fuelTypes;

    public void Init(Fuel fuel)
    {
        Material material = _fuelTypes.GetMaterialOf(fuel);

        foreach (Renderer pieceRenderer in GetComponentsInChildren<Renderer>())
            pieceRenderer.material = material;
    }
}
