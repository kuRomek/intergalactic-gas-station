using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LeaderboardWindow : UIMenu
    {
        [SerializeField] private Button _backButton;
        [SerializeField] private SettingsWindow _settingsWindow;

        private void OnEnable()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveAllListeners();
        }

        private void OnBackButtonClicked()
        {
            _settingsWindow.Show();
        }
    }
}
