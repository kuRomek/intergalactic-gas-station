using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;

namespace IntergalacticGasStation
{
    namespace UI
    {
        public class SettingsWindow : UIMenu
        {
            [SerializeField] private UIMenu _startScreen;
            [SerializeField] private UIMenu _leaderboardWindow;
            [SerializeField] private Button _backButton;
            [SerializeField] private Button _languageButtonForward;
            [SerializeField] private Button _languageButtonBackwardward;
            [SerializeField] private Button _leaderBoardButton;
            [SerializeField] private TextMeshProUGUI _language;
            [SerializeField] private Slider _soundVolumeSlider;
            [SerializeField] private Slider _musicVolumeSlider;
            [SerializeField] private Settings _settings;
            [SerializeField] private RectTransform _athorizedView;
            [SerializeField] private RectTransform _notAthorizedView;

            private void Awake()
            {
                if (YandexGame.auth == true)
                {
                    _athorizedView.gameObject.SetActive(true);
                    _notAthorizedView.gameObject.SetActive(false);
                }
                else
                {
                    _athorizedView.gameObject.SetActive(false);
                    _notAthorizedView.gameObject.SetActive(true);
                }

                _soundVolumeSlider.value = _settings.SoundVolume;
                _musicVolumeSlider.value = _settings.MusicVolume;

                _language.text = _settings.AvailableLanguages[YandexGame.lang];
            }

            private void OnEnable()
            {
                _backButton.onClick.AddListener(OnBackButtonClicked);
                _languageButtonForward.onClick.AddListener(() => OnLanguageButtonClicked(true));
                _languageButtonBackwardward.onClick.AddListener(() => OnLanguageButtonClicked(false));
                _leaderBoardButton.onClick.AddListener(_leaderboardWindow.Show);

                _soundVolumeSlider.onValueChanged.AddListener((_) => _settings.SetSoundVolume(_soundVolumeSlider.value));
                _musicVolumeSlider.onValueChanged.AddListener((_) => _settings.SetMusicVolume(_musicVolumeSlider.value));
            }

            private void OnDisable()
            {
                _backButton.onClick.RemoveAllListeners();
                _languageButtonForward.onClick.RemoveAllListeners();
                _languageButtonBackwardward.onClick.RemoveAllListeners();
                _leaderBoardButton.onClick.RemoveAllListeners();

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
    }
}
