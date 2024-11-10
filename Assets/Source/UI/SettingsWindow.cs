using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SettingsWindow : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _languageButtonForward;
    [SerializeField] private Button _languageButtonBackwardward;
    [SerializeField] private TextMeshProUGUI _language;
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
        _languageButtonForward.onClick.AddListener(() => OnLanguageButtonClicked(true));
        _languageButtonBackwardward.onClick.AddListener(() => OnLanguageButtonClicked(false));

        _soundVolumeSlider.onValueChanged.AddListener((_) => _settings.SetSoundVolume(_soundVolumeSlider.value));
        _musicVolumeSlider.onValueChanged.AddListener((_) => _settings.SetMusicVolume(_musicVolumeSlider.value));
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveAllListeners();
        _languageButtonForward.onClick.RemoveAllListeners();
        _languageButtonBackwardward.onClick.RemoveAllListeners();

        _soundVolumeSlider.onValueChanged.RemoveAllListeners();
        _musicVolumeSlider.onValueChanged.RemoveAllListeners();
    }

    private void OnBackButtonClicked()
    {
        _startScreen.Show();
    }

    private void OnLanguageButtonClicked(bool isForward = true)
    {
        int shift = isForward ? 1 : -1;
        string nextLanguage;
        string[] languageIds = new string[_settings.AvailableLanguages.Keys.Count()];

        int i = 0;

        foreach (string languageId in _settings.AvailableLanguages.Keys)
            languageIds[i++] = languageId;

        int languageIndex = (Array.IndexOf(languageIds, YandexGame.lang) + shift) % languageIds.Length;

        if (languageIndex >= 0)
            nextLanguage = languageIds[languageIndex];
        else
            nextLanguage = languageIds[^1];

        YandexGame.SwitchLanguage(nextLanguage);
        _language.text = _settings.AvailableLanguages[nextLanguage];
    }
}
