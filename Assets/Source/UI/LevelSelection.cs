using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private Button[] _levelButtons;
    [SerializeField] private Button _backButton;

    private void OnEnable()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
            _levelButtons[i].onClick.AddListener(() => LoadLevel(i));

        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        foreach (Button levelButton in _levelButtons)
            levelButton.onClick.RemoveAllListeners();

        _backButton.onClick.RemoveAllListeners();
    }

    private void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    private void OnBackButtonClicked()
    {
        Hide();
        _startScreen.Show();
    }
}
