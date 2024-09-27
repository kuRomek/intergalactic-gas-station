using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private PlayerInput _input;
    private Vector2 _lastMousePosition;

    public event Action<PipeTemplate> ButtonPressed;
    public event Action<Vector3> Dragging;
    public event Action DragCanceled;

    private void Awake()
    {
        _input = new PlayerInput();

        _input.Player.Press.performed += OnButtonPressed;
        _input.Player.Drag.performed += OnDragging;
        _input.Player.Press.canceled += OnDragCanceld;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void OnButtonPressed(InputAction.CallbackContext context)
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity))
        {
            PipeTemplatePresenter pipeTemplate = hit.collider.GetComponentInParent<PipeTemplatePresenter>();

            if (pipeTemplate != null)
                ButtonPressed?.Invoke(pipeTemplate.Model);

            _lastMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnDragging(InputAction.CallbackContext context)
    {
        Vector2 newMousePosition = _camera.ScreenToWorldPoint(context.action.ReadValue<Vector2>());
        Dragging?.Invoke(newMousePosition - _lastMousePosition);
        _lastMousePosition = newMousePosition;
    }

    private void OnDragCanceld(InputAction.CallbackContext context)
    {
        DragCanceled?.Invoke();
    }
}
