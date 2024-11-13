using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class StartScreen : UIMenu
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _infiniteGameButton;
    [SerializeField] private Button _settingButton;
    [SerializeField] private Image _lockInfiniteGame;
    [SerializeField] private UIMenu _levelSelection;
    [SerializeField] private UIMenu _settings;

    private TextMeshProUGUI _infiniteGameButtonText;
    private bool _isFirstLoad = true;

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

        _isFirstLoad = false;
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingButton.onClick.AddListener(OnSettingsButtonClicked);

        if (YandexGame.savesData.IsInfiniteGameUnlocked)
            _infiniteGameButton.onClick.AddListener(OnInfiniteGameButtonClicked);

        YandexGame.GetDataEvent += OnAuthorized;
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingButton.onClick.RemoveAllListeners();

        if (YandexGame.savesData.IsInfiniteGameUnlocked)
            _infiniteGameButton.onClick.RemoveAllListeners();

        YandexGame.GetDataEvent -= OnAuthorized;
    }

    private void OnAuthorized()
    {
        if (_isFirstLoad == false)
        {
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

            if (YandexGame.savesData.IsInfiniteGameUnlocked)
                _infiniteGameButton.onClick.AddListener(OnInfiniteGameButtonClicked);
        }
    }

    private void OnPlayButtonClicked()
    {
        _levelSelection.Show();
    }

    private void OnInfiniteGameButtonClicked()
    {
        SceneManager.LoadScene("InfiniteGame");
    }

    private void OnSettingsButtonClicked()
    {
        _settings.Show();
    }
}
