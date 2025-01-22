using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class AuthorizationAskWindow : MonoBehaviour
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;

        private void OnEnable()
        {
            if (YandexGame.auth == true)
                gameObject.SetActive(false);

            _yesButton.onClick.AddListener(YandexGame.AuthDialog);
            _noButton.onClick.AddListener(() => gameObject.SetActive(false));
        }

        private void OnDisable()
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
        }
    }
}
