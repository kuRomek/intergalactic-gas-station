using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class LevelCompleteWindow : UIMenu
    {
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Button _mainMenuButton;

        private void OnEnable()
        {
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
            _mainMenuButton.onClick.AddListener(LoadMainMenu);
        }

        private void OnDisable()
        {
            _nextLevelButton.onClick.RemoveAllListeners();
            _mainMenuButton.onClick.RemoveAllListeners();
        }

        private void OnNextLevelButtonClicked()
        {
            YandexGame.FullscreenShow();

            if (YandexGame.timerShowAd <= 0)
            {
                YandexGame.CloseFullAdEvent += LoadNextLevel;
            }
            else
            {
                Hide();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        private void LoadNextLevel()
        {
            YandexGame.CloseFullAdEvent -= LoadNextLevel;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
