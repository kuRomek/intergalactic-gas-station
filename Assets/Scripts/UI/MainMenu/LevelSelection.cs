using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelection : UIMenu
    {
        [SerializeField] private UIMenu _startScreen;
        [SerializeField] private LevelButton[] _levelButtons;
        [SerializeField] private Button _backButton;

        private void OnEnable()
        {
            for (int i = 0; i < _levelButtons.Length; i++)
                _levelButtons[i].Clicked += SceneManager.LoadScene;

            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            foreach (LevelButton levelButton in _levelButtons)
                levelButton.Clicked -= SceneManager.LoadScene;

            _backButton.onClick.RemoveAllListeners();
        }

        private void OnBackButtonClicked()
        {
            _startScreen.Show();
        }
    }
}
