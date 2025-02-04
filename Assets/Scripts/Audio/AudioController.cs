using UnityEngine;
using Settings;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundAudioSource;
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private GameRuntimeSettings _settings;

        private float _minPitch = 0.8f;
        private float _maxPitch = 1.2f;

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
            _soundAudioSource.pitch = Random.Range(_minPitch, _maxPitch);
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
