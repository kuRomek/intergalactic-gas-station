using UnityEngine;

public class PipeTemplateView : View
{
    [SerializeField] private FuelColors _fuelTypes;

    private Renderer[] _piecesRenderers = null;

    public void SetColor(Fuel fuel)
    {
        Material material = _fuelTypes.GetMaterialOf(fuel);

        _piecesRenderers ??= GetComponentsInChildren<Renderer>();

        foreach (Renderer pieceRenderer in _piecesRenderers)
            pieceRenderer.material = material;
    }
}
