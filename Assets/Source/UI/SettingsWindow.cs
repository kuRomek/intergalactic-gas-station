using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Settings _settings;

    private void Awake()
    {
        _soundVolumeSlider.value = _settings.SoundVolume;
        _musicVolumeSlider.value = _settings.MusicVolume;
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);

        _soundVolumeSlider.onValueChanged.AddListener((_) => _settings.SetSoundVolume(_soundVolumeSlider.value));
        _musicVolumeSlider.onValueChanged.AddListener((_) => _settings.SetMusicVolume(_musicVolumeSlider.value));
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveAllListeners();

        _soundVolumeSlider.onValueChanged.RemoveAllListeners();
        _musicVolumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void OnBackButtonClicked()
    {
        Hide();
        _startScreen.Show();
    }
}
