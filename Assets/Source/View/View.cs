using UnityEngine;

public class View : MonoBehaviour
{
    private Audio _audio = null;

    public void PlaySound(AudioClip clip)
    {
        if (_audio != null)
        {
            _audio.PlaySound(clip);
        }
        else
        {
            _audio = FindAnyObjectByType<Audio>();
            _audio.PlaySound(clip);
        }
    }
}
