using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : UIMenu
{
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _levelSelectionButton;

    private void OnEnable()
    {
        _tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        _levelSelectionButton.onClick.AddListener(OnLevelSelectionButtonClicked);
    }

    private void OnDisable()
    {
        _tryAgainButton.onClick.RemoveAllListeners();
        _levelSelectionButton.onClick.RemoveAllListeners();
    }

    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }

    private void OnLevelSelectionButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
