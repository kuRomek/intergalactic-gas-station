using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

public class LoseWindow : UIMenu
{
    [SerializeField] private Button _rewardedAdButton;
    [SerializeField] private Button _tryAgainButton;
    [SerializeField] private Button _levelSelectionButton;

    private bool _hasWatchedRewardedVideo = false;

    private void OnEnable()
    {
        if (_hasWatchedRewardedVideo == false)
        {
            _rewardedAdButton.onClick.AddListener(OnRewardedVideoButtonClicked);
            _tryAgainButton.onClick.AddListener(OnTryAgainButtonClicked);
        }
        else
        {
            _rewardedAdButton.gameObject.SetActive(false);
        }

        _levelSelectionButton.onClick.AddListener(OnLevelSelectionButtonClicked);
    }

    private void OnDisable()
    {
        _rewardedAdButton.onClick.RemoveAllListeners();
        _tryAgainButton.onClick.RemoveAllListeners();
        _levelSelectionButton.onClick.RemoveAllListeners();
    }

    private void OnRewardedVideoButtonClicked()
    {
        YandexGame.RewVideoShow(1);
        _hasWatchedRewardedVideo = true;
    }

    private void OnTryAgainButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().path);
    }

    private void OnLevelSelectionButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
