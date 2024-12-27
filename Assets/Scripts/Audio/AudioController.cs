using UnityEngine;

namespace IntergalacticGasStation
{
    namespace Audio
    {
        public class AudioController : MonoBehaviour
        {
            [SerializeField] private AudioSource _soundAudioSource;
            [SerializeField] private AudioSource _musicAudioSource;
            [SerializeField] private Settings _settings;

            private void Awake()
            {
                SetSoundVolume();
                SetMusicVolume();
            }

            private void OnEnable()
            {
                _settings.SoundVolumeChanged += SetSoundVolume;
                _settings.MusicVolumeChanged += SetMusicVolume;
                Application.focusChanged += OnFocusChanged;
            }

            private void OnDisable()
            {
                _settings.SoundVolumeChanged -= SetSoundVolume;
                _settings.MusicVolumeChanged -= SetMusicVolume;
                Application.focusChanged -= OnFocusChanged;
            }

            public void PlaySound(AudioClip audioClip)
            {
                _soundAudioSource.pitch = Random.Range(0.8f, 1.2f);
                _soundAudioSource.PlayOneShot(audioClip);
            }

            private void SetSoundVolume()
            {
                _soundAudioSource.volume = _settings.SoundVolume;
            }

            private void SetMusicVolume()
            {
                _musicAudioSource.volume = _settings.MusicVolume;
            }

            private void OnFocusChanged(bool isVisible)
            {
                if (isVisible)
                {
                    _musicAudioSource.UnPause();
                    _soundAudioSource.UnPause();
                }
                else
                {
                    _musicAudioSource.Pause();
                    _soundAudioSource.Pause();
                }
            }
        }
    }
}
