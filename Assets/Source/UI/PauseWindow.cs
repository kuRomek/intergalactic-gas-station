using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWindow : UIMenu
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _levelSelectionButton;

    private void OnEnable()
    {
        _resumeButton.onClick.AddListener(Hide);
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _levelSelectionButton.onClick.AddListener(OnLevelSelectionButtonClicked);
    }

    private void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(Hide);
        _restartButton.onClick.RemoveAllListeners();
        _levelSelectionButton.onClick.RemoveAllListeners();
    }

    private void OnRestartButtonClicked()
    {
        Hide();
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }

    private void OnLevelSelectionButtonClicked()
    {
        Hide();
        SceneManager.LoadScene(0);
    }
}
