using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : UIMenu
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _infiniteGameButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private UIMenu _levelSelection;
    [SerializeField] private UIMenu _settings;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _infiniteGameButton.onClick.AddListener(OnInfiniteGameButtonClicked);
        _settingButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
        _infiniteGameButton.onClick.RemoveAllListeners();
        _settingButton.onClick.RemoveAllListeners();
    }

    private void OnPlayButtonClicked()
    {
        Hide();
        _levelSelection.Show();
    }

    private void OnInfiniteGameButtonClicked()
    {
        SceneManager.LoadScene("InfiniteGame");
    }

    private void OnSettingsButtonClicked()
    {
        Hide();
        _settings.Show();
    }
}
