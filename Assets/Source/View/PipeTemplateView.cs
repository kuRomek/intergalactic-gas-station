using UnityEngine;

public class PipeTemplateView : View
{
    [SerializeField] private FuelColors _fuelTypes;
    private AudioClip _placedSound = null;

    private Renderer[] _piecesRenderers = null;

    private void Awake()
    {
        _placedSound = (AudioClip)Resources.Load("Sounds/pipe");
    }

    public void SetColor(Fuel fuel)
    {
        Material material = _fuelTypes.GetMaterialOf(fuel);

        _piecesRenderers ??= GetComponentsInChildren<Renderer>();

        foreach (Renderer pieceRenderer in _piecesRenderers)
            pieceRenderer.material = material;
    }

    public void PlaySoundOnPlaced()
    {
        if (_placedSound != null)
            PlaySound(_placedSound);
    }
}
