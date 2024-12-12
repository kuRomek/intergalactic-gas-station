using System;
using UnityEngine;

public abstract class UIMenu : MonoBehaviour
{
    public event Action Showed;

    public event Action Hid;

    public void Show()
    {
        gameObject.SetActive(true);
        Showed?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        Hid?.Invoke();
    }
}
