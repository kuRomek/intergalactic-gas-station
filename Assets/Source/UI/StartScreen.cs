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
    [SerializeField] private UIMenu _levelSelection;
    [SerializeField] private UIMenu _settings;

    private Color _lockedButtonColor = new Color(58f / 256f, 13f / 256f, 13f / 256f);

    private void Awake()
    {
        TextMeshProUGUI buttonText = _infiniteGameButton.GetComponentInChildren<TextMeshProUGUI>();

        if (YandexGame.savesData.IsInfiniteGameUnlocked)
        {
            buttonText.color = Color.white;
            buttonText.text = "Infinite\nGame";
        }
        else
        {
            buttonText.color = _lockedButtonColor;
            buttonText.text = "Locked";
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
        SceneManager.LoadScene("InfiniteGame");
    }

    private void OnSettingsButtonClicked()
    {
        _settings.Show();
    }
}
