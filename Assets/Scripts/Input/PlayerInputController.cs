using System;
using UnityEngine;
using UnityEngine.InputSystem;
using IntergalacticGasStation.LevelControl;
using IntergalacticGasStation.Pipes;

namespace IntergalacticGasStation
{
    namespace Input
    {
        public class PlayerInputController : MonoBehaviour
        {
            [SerializeField] private Camera _camera;

            private LevelState _levelState;
            private PlayerInput _input;
            private Vector2 _lastMousePosition;

            public event Action<PipeTemplate> DragStarted;

            public event Action<Vector3> Dragging;

            public event Action DragCanceled;

            private void Awake()
            {
                _input = new PlayerInput();
            }

            private void OnEnable()
            {
                _input.Enable();

                _input.Player.Press.performed += OnButtonPressed;
                _input.Player.Drag.performed += OnDragging;
                _input.Player.Press.canceled += OnDragCanceld;
            }

            private void OnDisable()
            {
                _input.Disable();

                _input.Player.Press.performed -= OnButtonPressed;
                _input.Player.Drag.performed -= OnDragging;
                _input.Player.Press.canceled -= OnDragCanceld;
            }

            public void Init(LevelState levelState)
            {
                _levelState = levelState;
            }

            private void OnButtonPressed(InputAction.CallbackContext context)
            {
                if (_levelState.IsGameOver || _levelState.IsPaused)
                    return;

                Ray ray = _camera.ScreenPointToRay(UnityEngine.Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity))
                {
                    PipeTemplatePresenter pipeTemplate = hit.collider.GetComponentInParent<PipeTemplatePresenter>();

                    if (pipeTemplate != null)
                        DragStarted?.Invoke(pipeTemplate.Model);

                    _lastMousePosition = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
                }
            }

            private void OnDragging(InputAction.CallbackContext context)
            {
                if (_levelState.IsGameOver || _levelState.IsPaused)
                    return;

                Vector2 newMousePosition = _camera.ScreenToWorldPoint(context.action.ReadValue<Vector2>());
                Dragging?.Invoke(newMousePosition - _lastMousePosition);
                _lastMousePosition = newMousePosition;
            }

            private void OnDragCanceld(InputAction.CallbackContext context)
            {
                if (_levelState.IsGameOver || _levelState.IsPaused)
                    return;

                DragCanceled?.Invoke();
            }
        }
    }
}
