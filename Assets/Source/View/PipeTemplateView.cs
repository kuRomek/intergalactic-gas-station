using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PipeTemplateView : View
{
    private const float DistanceTolerance = 0.05f;

    [SerializeField] private FuelColors _fuelTypes;

    private Outline _outline;
    private AudioClip _placedSound = null;
    private Renderer[] _piecesRenderers = null;
    private float _maxOutlineWidth = 8f;
    private Coroutine _changingOutlineWidth;

    private void Awake()
    {
        _placedSound = (AudioClip)Resources.Load("Sounds/pipe");
        _outline = GetComponent<Outline>();

        _outline.OutlineWidth = 0f;
        _outline.OutlineColor = Color.white;
    }

    public void SetOutline()
    {
        if (_changingOutlineWidth != null)
            StopCoroutine(_changingOutlineWidth);

        _changingOutlineWidth = StartCoroutine(ChangeOutlineWidth(_maxOutlineWidth));
    }

    public void RemoveOutline()
    {
        if (_changingOutlineWidth != null)
            StopCoroutine(_changingOutlineWidth);

        _changingOutlineWidth = StartCoroutine(ChangeOutlineWidth(0f));
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
        if (_outline != null)
        {
            _outline.enabled = !_outline.enabled;
            _outline.enabled = !_outline.enabled;
        }

        if (_placedSound != null)
            PlaySound(_placedSound);
    }

    private IEnumerator ChangeOutlineWidth(float endWidth)
    {
        while (Mathf.Abs(_outline.OutlineWidth - endWidth) > DistanceTolerance)
        {
            _outline.OutlineWidth = Mathf.Lerp(_outline.OutlineWidth, endWidth, Time.deltaTime * 9f);
            yield return null;
        }

        _outline.OutlineWidth = endWidth;
    }
}
