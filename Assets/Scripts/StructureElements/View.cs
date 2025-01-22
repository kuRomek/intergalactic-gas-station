using UnityEngine;
using Audio;

namespace StructureElements
{
    public class View : MonoBehaviour
    {
        private AudioController _audio = null;

        public void PlaySound(AudioClip clip)
        {
            if (_audio != null)
            {
                _audio.PlaySound(clip);
            }
            else
            {
                _audio = FindAnyObjectByType<AudioController>();
                _audio.PlaySound(clip);
            }
        }
    }
}
