using UnityEngine;
using UnityEngine.SceneManagement;
using YG;
using Input;
using Pipes;
using PlayerProgress;

namespace Misc
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private PlayerInputController _playerInputController;

        private int _levelNumber;

        private void Awake()
        {
            _levelNumber = SceneManager.GetActiveScene().buildIndex;

            if (YandexGame.savesData.ShownTutorials[_levelNumber - 1] == true)
                gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _playerInputController.DragStarted += Hide;
        }

        private void OnDisable()
        {
            _playerInputController.DragStarted -= Hide;
        }

        private void Hide(PipeTemplate _)
        {
            PlayerProgressController.RemoveTutorialOnLevel(_levelNumber);
            gameObject.SetActive(false);
        }
    }
}
