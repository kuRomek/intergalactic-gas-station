using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class PauseWindow : UIMenu
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(Hide);
        _restartButton.onClick.AddListener(RestartLevel);
        _mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(Hide);
        _restartButton.onClick.RemoveAllListeners();
        _mainMenuButton.onClick.RemoveAllListeners();
    }

    private void RestartLevel()
    {
        Hide();
        YandexGame.FullscreenShow();
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
