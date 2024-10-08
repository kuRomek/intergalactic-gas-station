using UnityEngine;
using UnityEngine.UI;

public class Settings : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private Button _backButton;
    [SerializeField] private Slider _soundVolumeSlider;
    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);

        _soundVolumeSlider.onValueChanged.AddListener((_) => _soundAudioSource.volume = _soundVolumeSlider.value);
        _musicVolumeSlider.onValueChanged.AddListener((_) => _musicAudioSource.volume = _musicVolumeSlider.value);

        _soundVolumeSlider.value = _soundAudioSource.volume;
        _musicVolumeSlider.value = _musicAudioSource.volume;
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
