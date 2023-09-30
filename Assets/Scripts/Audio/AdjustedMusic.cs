using UnityEngine;

[CreateAssetMenu(menuName = "Music")]
public class AdjustedMusic : ScriptableObject
{
    [SerializeField] private AudioClip _clip = null;
    [SerializeField] private float _volume = 0.3f;

    public AudioClip Clip => _clip;
    public float Volume => _volume;
}