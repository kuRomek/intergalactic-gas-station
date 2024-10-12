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
        //for (int i = 0; i < _levelButtons.Length; i++)
        //    _levelButtons[i].onClick.AddListener(() => SceneManager.LoadScene(i + 1));

        _levelButtons[0].onClick.AddListener(() => SceneManager.LoadScene(1));
        _levelButtons[1].onClick.AddListener(() => SceneManager.LoadScene(2));
        _levelButtons[2].onClick.AddListener(() => SceneManager.LoadScene(3));
        _levelButtons[3].onClick.AddListener(() => SceneManager.LoadScene(4));
        _levelButtons[4].onClick.AddListener(() => SceneManager.LoadScene(5));
        _levelButtons[5].onClick.AddListener(() => SceneManager.LoadScene(6));
        _levelButtons[6].onClick.AddListener(() => SceneManager.LoadScene(7));
        _levelButtons[7].onClick.AddListener(() => SceneManager.LoadScene(8));
        _levelButtons[8].onClick.AddListener(() => SceneManager.LoadScene(9));
        _levelButtons[9].onClick.AddListener(() => SceneManager.LoadScene(10));

        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        foreach (Button levelButton in _levelButtons)
            levelButton.onClick.RemoveAllListeners();

        _backButton.onClick.RemoveAllListeners();
    }

    private void OnBackButtonClicked()
    {
        Hide();
        _startScreen.Show();
    }
}
