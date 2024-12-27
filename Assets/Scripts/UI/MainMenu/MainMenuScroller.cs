using System.Collections;
using UnityEngine;
using IntergalacticGasStation.Misc;

namespace IntergalacticGasStation
{
    namespace UI
    {
        public class MainMenuScroller : MonoBehaviour
        {
            private const float DistanceTolerance = 0.1f;
            private const float CameraShiftDivider = 300f;

            [SerializeField] private float _scrollingSpeed = 5f;
            [SerializeField] private BackgroundWiggler _background;
            [SerializeField] private UIMenu _startScreen;
            [SerializeField] private UIMenu _levelSelectionScreen;
            [SerializeField] private UIMenu _settingsScreen;
            [SerializeField] private UIMenu _leaderboardWindow;
            [SerializeField] private Camera _camera;

            private Coroutine _scrolling;
            private RectTransform _rectTransform;
            private Vector3 _startScreenLocation = Vector3.zero;
            private Vector3 _levelSelectionScreenLocation = new Vector3(0f, -3000f, 0f);
            private Vector3 _settingsScreenLocation = new Vector3(0f, 3000f, 0f);
            private Vector3 _leaderboardScreenLocation = new Vector3(0f, 6000f, 0f);

            private void Awake()
            {
                _rectTransform = GetComponent<RectTransform>();
            }

            private void OnEnable()
            {
                _startScreen.Showed += ScrollToStartScreen;
                _levelSelectionScreen.Showed += ScrollToLevelSelectionScreen;
                _settingsScreen.Showed += ScrollToSettingsScreen;
                _leaderboardWindow.Showed += ScrollToLeaderboardScreen;
            }

            private void OnDisable()
            {
                _startScreen.Showed -= ScrollToStartScreen;
                _levelSelectionScreen.Showed -= ScrollToLevelSelectionScreen;
                _settingsScreen.Showed -= ScrollToSettingsScreen;
                _leaderboardWindow.Showed += ScrollToLeaderboardScreen;
            }

            private void ScrollToStartScreen() =>
                ScrollTo(_startScreenLocation);

            private void ScrollToLevelSelectionScreen() =>
                ScrollTo(_levelSelectionScreenLocation);

            private void ScrollToSettingsScreen() =>
                ScrollTo(_settingsScreenLocation);

            private void ScrollToLeaderboardScreen() =>
                ScrollTo(_leaderboardScreenLocation);

            private void ScrollTo(Vector3 position)
            {
                if (_scrolling != null)
                    StopCoroutine(_scrolling);

                _scrolling = StartCoroutine(StartScrollingTo(position));
            }

            private IEnumerator StartScrollingTo(Vector3 position)
            {
                while (Vector3.SqrMagnitude(_rectTransform.localPosition - position) > DistanceTolerance)
                {
                    _rectTransform.localPosition = Vector3.Lerp(
                        _rectTransform.localPosition,
                        position,
                        _scrollingSpeed * Time.deltaTime);

                    _camera.transform.position = Vector3.Lerp(
                        _camera.transform.position,
                        (-position / CameraShiftDivider) + (Vector3.forward * _camera.transform.position.z),
                        _scrollingSpeed * Time.deltaTime);

                    yield return null;
                }

                _rectTransform.localPosition = position;
            }
        }
    }
}
