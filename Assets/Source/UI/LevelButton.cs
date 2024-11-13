using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _number;
    [SerializeField] private Image _lockImage;

    private int _levelNumber;

    public event Action<int> Clicked;

    private void Awake()
    {
        _levelNumber = Convert.ToInt32(_number.text);

        bool isLevelUnlocked = YandexGame.savesData.OpenLevels[_levelNumber - 1] == true;

        _number.gameObject.SetActive(isLevelUnlocked);
        _lockImage.gameObject.SetActive(isLevelUnlocked == false);
    }

    private void OnEnable()
    {
        if (YandexGame.savesData.OpenLevels[_levelNumber - 1] == true)
            _button.onClick.AddListener(OnClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void OnAuthorized()
    {
        bool isLevelUnlocked = YandexGame.savesData.OpenLevels[_levelNumber - 1] == true;

        _number.gameObject.SetActive(isLevelUnlocked);
        _lockImage.gameObject.SetActive(isLevelUnlocked == false);

        if (YandexGame.savesData.OpenLevels[_levelNumber - 1] == true)
            _button.onClick.AddListener(OnClicked);
    }

    private void OnClicked()
    {
        Clicked?.Invoke(_levelNumber);
    }
}
