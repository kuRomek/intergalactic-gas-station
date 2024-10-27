using System;
using UnityEngine;

[CreateAssetMenu(fileName="Settings", menuName="Settings", order=56)]
public class Settings : ScriptableObject
{
    [SerializeField, Range(0f, 1f)] private float _soundVolume;
    [SerializeField, Range(0f, 1f)] private float _musicVolume;

    public event Action SoundVolumeChanged;
    public event Action MusicVolumeChanged;

    public float SoundVolume => _soundVolume;
    public float MusicVolume => _musicVolume;

    public void SetSoundVolume(float volume)
    {
        _soundVolume = volume;
        SoundVolumeChanged?.Invoke();
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = volume;
        MusicVolumeChanged?.Invoke();
    }
}
