using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using YG;

namespace UI
{
    public class StartScreen : UIMenu
    {
        private const string InfiniteGame = nameof(InfiniteGame);

        [SerializeField] private Button _playButton;
        [SerializeField] private Button _infiniteGameButton;
        [SerializeField] private Button _settingButton;
        [SerializeField] private Image _lockInfiniteGame;
        [SerializeField] private UIMenu _levelSelection;
        [SerializeField] private UIMenu _settings;

        private TextMeshProUGUI _infiniteGameButtonText;

        private void Awake()
        {
            _infiniteGameButtonText = _infiniteGameButton.GetComponentInChildren<TextMeshProUGUI>(true);

            if (YandexGame.savesData.IsInfiniteGameUnlocked)
            {
                _infiniteGameButtonText.gameObject.SetActive(true);
                _lockInfiniteGame.gameObject.SetActive(false);
            }
            else
            {
                _infiniteGameButtonText.gameObject.SetActive(false);
                _lockInfiniteGame.gameObject.SetActive(true);
            }

            Time.timeScale = 1f;
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _settingButton.onClick.AddListener(OnSettingsButtonClicked);

            if (YandexGame.savesData.IsInfiniteGameUnlocked)
                _infiniteGameButton.onClick.AddListener(OnInfiniteGameButtonClicked);
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveAllListeners();
            _settingButton.onClick.RemoveAllListeners();

            if (YandexGame.savesData.IsInfiniteGameUnlocked)
                _infiniteGameButton.onClick.RemoveAllListeners();
        }

        private void OnPlayButtonClicked()
        {
            _levelSelection.Show();
        }

        private void OnInfiniteGameButtonClicked()
        {
            SceneManager.LoadScene(InfiniteGame);
        }

        private void OnSettingsButtonClicked()
        {
            _settings.Show();
        }
    }
}
