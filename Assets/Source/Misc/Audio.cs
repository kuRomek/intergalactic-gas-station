using UnityEngine;
using YG;

public class Audio : MonoBehaviour
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
        YandexGame.onHideWindowGame += Mute;
        YandexGame.onShowWindowGame += Unmute;
    }

    private void OnDisable()
    {
        _settings.SoundVolumeChanged -= SetSoundVolume;
        _settings.MusicVolumeChanged -= SetMusicVolume;
        YandexGame.onHideWindowGame -= Mute;
        YandexGame.onShowWindowGame -= Unmute;
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

    private void Mute()
    {
        _soundAudioSource.mute = true;
        _musicAudioSource.mute = true;
        AudioListener.pause = true;
    }

    private void Unmute()
    {
        _soundAudioSource.mute = false;
        _musicAudioSource.mute = false;
        AudioListener.pause = false;
    }
}
