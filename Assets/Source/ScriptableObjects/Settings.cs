using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Settings", menuName="Settings", order=56)]
public class Settings : ScriptableObject
{
    [SerializeField, Range(0f, 1f)] private float _soundVolume;
    [SerializeField, Range(0f, 1f)] private float _musicVolume;

    private Dictionary<string, string> _availableLanguages = new Dictionary<string, string>()
    {
        { "en", "English" },
        { "ru", "Русский" },
        { "tr", "Türk" },
    };

    public event Action SoundVolumeChanged;

    public event Action MusicVolumeChanged;

    public float SoundVolume => _soundVolume;

    public float MusicVolume => _musicVolume;

    public IReadOnlyDictionary<string, string> AvailableLanguages => _availableLanguages;

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
