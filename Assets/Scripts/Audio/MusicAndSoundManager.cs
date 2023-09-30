using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAndSoundManager : Singleton<MusicAndSoundManager>
{
    [SerializeField] float _minRandomPitch = .9f;
    [SerializeField] float _maxRandomPitch = 1.1f;
    [SerializeField] Sound[] _soundsArray = null;

    [SerializeField] SoundAudioSource _soundAudioSourcePrefab = null;

    private readonly Dictionary<ESoundName, List<Sound>> _soundsDict = new Dictionary<ESoundName, List<Sound>>();
    private readonly List<SoundAudioSource> _soundAudioSources = new List<SoundAudioSource>();

    private AudioSource _musicAudioSource = null;
    private float _initialMusicVolume;


    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        _musicAudioSource = GetComponent<AudioSource>();
        _initialMusicVolume = _musicAudioSource.volume;

        AdjustMusicVolume();

        _soundsDict.Clear();
        foreach (var sound in _soundsArray)
        {
            if (!_soundsDict.ContainsKey(sound.Name))
            {
                _soundsDict[sound.Name] = new List<Sound>();
            }
            _soundsDict[sound.Name].Add(sound);
        }
    }

    public static void TriggerUpdate()
    {
        _instance.AdjustMusicVolume();
    }


    public static void PlayMusic()
    {
        _instance._musicAudioSource.Play();
    }

    public static void ResetMusic()
    {
        _instance._musicAudioSource.Stop();
    }

    public static void FadeOutMusic()
    {
        _instance.StartCoroutine(_instance.FadeOutMusicCoroutine());
    }

    private IEnumerator FadeOutMusicCoroutine()
    {
        var timer = 2f;
        var initialVolume = _musicAudioSource.volume;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _musicAudioSource.volume = timer / 2f * initialVolume;
            yield return new WaitForEndOfFrame();
        }
    }

    public static void SetMusic(AdjustedMusic newMusic)
    {
        _instance._musicAudioSource.clip = newMusic.Clip;
        _instance._musicAudioSource.volume = newMusic.Volume;
        PlayMusic();
    }

    public static void ChangeMusic(AdjustedMusic newMusic)
    {
        _instance.StartCoroutine(_instance.ChangeMusicCoroutine(newMusic.Clip, newMusic.Volume));
    }

    private IEnumerator ChangeMusicCoroutine(AudioClip newMusic, float newVolume)
    {
        var timer = .5f;
        var initialVolume = _musicAudioSource.volume;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            _musicAudioSource.volume = timer * 2f * initialVolume;
            yield return new WaitForEndOfFrame();
        }

        _instance._musicAudioSource.clip = newMusic;
        PlayMusic();

        while (timer <= .5f)
        {
            timer += Time.unscaledDeltaTime;
            _musicAudioSource.volume = timer * 2f * newVolume;
            yield return new WaitForEndOfFrame();
        }

        _musicAudioSource.volume = newVolume;
    }

    public static void PlaySound(ESoundName soundName, float volumeMultiplier = 1f, float pitchMultiplier = 1f)
    {
        if (!_instance._soundsDict.TryGetValue(soundName, out var soundList))
        {
            Debug.Log("No sound named: " + soundName);
        }
        else
        {
            var sound = soundList[Random.Range(0, soundList.Count)];

            float volume = sound.Volume;
            volume *= volumeMultiplier;

            float pitch = sound.ShouldRandomPitch ? Random.Range(_instance._minRandomPitch, _instance._maxRandomPitch) : 1f;
            pitch *= pitchMultiplier;

            foreach (var soundAudioSource in _instance._soundAudioSources)
            {
                if (soundAudioSource.IsAvailable)
                {
                    soundAudioSource.PlaySound(sound.Clip, volume, pitch);
                    return;
                }
            }

            SoundAudioSource newAudioSource = Instantiate(_instance._soundAudioSourcePrefab, _instance.transform);
            _instance._soundAudioSources.Add(newAudioSource);
            newAudioSource.PlaySound(sound.Clip, volume, pitch);
        }
    }

    private void AdjustMusicVolume()
    {
        if (_musicAudioSource != null)
        {
            _musicAudioSource.volume = _initialMusicVolume;
        }
    }
}