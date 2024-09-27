using UnityEngine;

public class Presenter : MonoBehaviour
{
    private Transformable _model;
    private IUpdatable _updatable = null;

    public Transformable Model => _model;

    private void Update()
    {
        _updatable?.Update(Time.deltaTime);
    }

    private void OnEnable()
    {
        _model.Moved += OnMoved;
        _model.ExceptionCaught += WriteExceptionMessage;
    }

    private void OnDisable()
    {
        _model.Moved -= OnMoved;
        _model.ExceptionCaught -= WriteExceptionMessage;
    }

    private void WriteExceptionMessage(string message)
    {
        Debug.Log(message);
    }

    public void Init(Transformable model)
    {
        _model = model;

        if (_model is IUpdatable updatable)
            _updatable = updatable;

        enabled = true;

        OnMoved();
        OnRotated();
    }

    private void OnMoved()
    {
        transform.position = _model.Position;
    }

    private void OnRotated()
    {
        transform.rotation = _model.Rotation;
    }
}
