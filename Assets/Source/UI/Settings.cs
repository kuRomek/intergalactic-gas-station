using UnityEngine;
using UnityEngine.UI;

public class Settings : UIMenu
{
    [SerializeField] private UIMenu _startScreen;
    [SerializeField] private Button _backButton;

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
        Hide();
        _startScreen.Show();
    }
}
