using UnityEngine;
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
        _settingButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _settingButton.onClick.RemoveListener(OnSettingsButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        Hide();
        _levelSelection.Show();
    }

    private void OnSettingsButtonClicked()
    {
        Hide();
        _settings.Show();
    }
}
