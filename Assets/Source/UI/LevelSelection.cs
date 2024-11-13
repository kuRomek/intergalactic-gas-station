using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LevelSelection : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private LevelButton[] _levelButtons;
    [SerializeField] private Button _backButton;

    private bool _isFirstLoad = true;

    private void Awake()
    {
        _isFirstLoad = false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
            _levelButtons[i].Clicked += SceneManager.LoadScene;

        _backButton.onClick.AddListener(OnBackButtonClicked);

        YandexGame.GetDataEvent += OnAuthorized;
    }

    private void OnDisable()
    {
        foreach (LevelButton levelButton in _levelButtons)
            levelButton.Clicked -= SceneManager.LoadScene;

        _backButton.onClick.RemoveAllListeners();

        YandexGame.GetDataEvent -= OnAuthorized;
    }

    private void OnAuthorized()
    {
        if (_isFirstLoad == false)
        {
            foreach (LevelButton levelButton in _levelButtons)
                levelButton.OnAuthorized();
        }
    }

    private void OnBackButtonClicked()
    {
        _startScreen.Show();
    }
}
