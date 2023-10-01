using UnityEngine;

public enum ESoundName
{
    None,

    Waf,
    Dash,
    Oof,
    Fireball,
    Boo1,
    Boo2,
    Boo3
}

[CreateAssetMenu(menuName = "Sound")]
public class Sound : ScriptableObject
{
    [SerializeField] private ESoundName _name = ESoundName.None;
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private float _volume = 0.5f;
    [SerializeField] private bool _shouldRandomPitch = false;

    public ESoundName Name => _name;
    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public bool ShouldRandomPitch => _shouldRandomPitch;
}