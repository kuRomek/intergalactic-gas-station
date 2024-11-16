using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private PlayerInputController _playerInputController;

    private int _levelNumber;

    private void Awake()
    {
        _levelNumber = SceneManager.GetActiveScene().buildIndex;

#if !UNITY_EDITOR
        if (YandexGame.savesData.ShownTutorials[_levelNumber - 1] == true)
            gameObject.SetActive(false);
#endif
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
