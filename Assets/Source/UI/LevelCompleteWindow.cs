using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelCompleteWindow : UIMenu
{
    [SerializeField] private Button _nextLevelButton;
    [SerializeField] private Button _levelSelectionButton;

    private void OnEnable()
    {
        _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        _levelSelectionButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnDisable()
    {
        _nextLevelButton.onClick.RemoveAllListeners();
        _levelSelectionButton.onClick.RemoveAllListeners();
    }

    private void OnNextLevelButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
