using UnityEngine;

public class Presenter : MonoBehaviour
{
    private Transformable _model;
    private View _view;
    private IUpdatable _updatable = null;
    private IActivatable _activatable = null;

    public Transformable Model => _model;
    public View View => _view;

    private void Update()
    {
        _updatable?.Update(Time.deltaTime);
    }

    private void OnEnable()
    {
        _model.Moved += OnMoved;
        _model.Rotated += OnRotated;
        _model.Destroying += OnDestroying;

        _activatable?.Enable();

        if (this is IActivatable activatable)
            activatable.Enable();
    }

    private void OnDisable()
    {
        _model.Moved -= OnMoved;
        _model.Rotated -= OnRotated;
        _model.Destroying -= OnDestroying;

        _activatable?.Disable();

        if (this is IActivatable activatable)
            activatable.Disable();
    }

    public void Init(Transformable model)
    {
        _model = model;
        _view = GetComponent<View>();

        if (_model is IUpdatable updatable)
            _updatable = updatable;

        if (_model is IActivatable activatable)
            _activatable = activatable;

        enabled = true;

        if (_view != null)
            _view.enabled = true;

        OnMoved();
        OnRotated();
        OnScaled();
    }

    private void OnMoved()
    {
        transform.position = _model.Position;
    }

    private void OnRotated()
    {
        transform.rotation = _model.Rotation;
    }

    private void OnScaled()
    {
        transform.localScale = _model.Scale;
    }

    private void OnDestroying()
    {
        Destroy(gameObject);
    }
}
