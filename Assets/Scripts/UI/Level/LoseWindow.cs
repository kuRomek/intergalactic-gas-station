using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace IntergalacticGasStation
{
    namespace UI
    {
        public class LoseWindow : UIMenu
        {
            [SerializeField] private Button _rewardedAdButton;
            [SerializeField] private Button _tryAgainButton;
            [SerializeField] private Button _mainMenuButton;

            private bool _hasWatchedRewardedVideo = false;

            private void OnEnable()
            {
                if (_hasWatchedRewardedVideo == false)
                    _rewardedAdButton.onClick.AddListener(ShowRewardedVideoAd);
                else
                    _rewardedAdButton.gameObject.SetActive(false);

                _tryAgainButton.onClick.AddListener(OnRestartButtonClicked);
                _mainMenuButton.onClick.AddListener(LoadMainMenu);
            }

            private void OnDisable()
            {
                _rewardedAdButton.onClick.RemoveAllListeners();
                _tryAgainButton.onClick.RemoveAllListeners();
                _mainMenuButton.onClick.RemoveAllListeners();
            }

            private void ShowRewardedVideoAd()
            {
                _rewardedAdButton.onClick.RemoveAllListeners();
                _rewardedAdButton.gameObject.SetActive(false);
                YandexGame.RewVideoShow(1);
                _hasWatchedRewardedVideo = true;
            }

            private void OnRestartButtonClicked()
            {
                YandexGame.FullscreenShow();

                if (YandexGame.timerShowAd <= 0)
                {
                    YandexGame.CloseFullAdEvent += Restart;
                }
                else
                {
                    Hide();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().path);
                }
            }

            private void Restart()
            {
                YandexGame.CloseFullAdEvent -= Restart;
                SceneManager.LoadScene(SceneManager.GetActiveScene().path);
            }

            private void LoadMainMenu()
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
