using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelCompleteWindow : UIMenu
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _mainMenuButton;

    private void OnEnable()
    {
        _nextLevelButton.onClick.AddListener(LoadNextLevel);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _nextLevelButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
    }

    private void LoadNextLevel()
    {
        YandexGame.FullscreenShow();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
