using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class PauseWindow : UIMenu
    {
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;

        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(Hide);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _mainMenuButton.onClick.AddListener(LoadMainMenu);
            YandexGame.CloseFullAdEvent += Hide;
        }

        private void OnDisable()
        {
            _resumeButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();
            YandexGame.CloseFullAdEvent -= Hide;
        }

        private void OnRestartButtonClicked()
        {
            YandexGame.FullscreenShow();

            if (YandexGame.timerShowAd <= 0)
            {
                YandexGame.CloseFullAdEvent += Restart;
            }
            else
            {
                Hide();
                SceneManager.LoadScene(SceneManager.GetActiveScene().path);
            }
        }

        private void Restart()
        {
            YandexGame.CloseFullAdEvent -= Restart;
            SceneManager.LoadScene(SceneManager.GetActiveScene().path);
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
