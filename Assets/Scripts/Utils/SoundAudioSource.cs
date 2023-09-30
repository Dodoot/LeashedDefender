using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource = null;

    private float _timer;
    private bool _isAvailable;

    public bool IsAvailable => _isAvailable;

    public void PlaySound(AudioClip clip, float volume, float pitch)
    {
        _audioSource.pitch = pitch;
        _audioSource.PlayOneShot(clip, volume);

        _isAvailable = false;
        _timer = clip.length / pitch;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.unscaledDeltaTime;
            if (_timer <= 0)
            {
                _isAvailable = true;
            }
        }
    }
}
