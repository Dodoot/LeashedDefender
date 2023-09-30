using UnityEngine;

public enum ESoundName
{
    None,

    HeroAttackMagical,
    HeroAttackPhysical,
    HeroAttackThrow,
    HeroChangeDirection,
    HeroJumpStart,
    HeroJumpEnd,
    HeroDash,
    HeroOuch,
    HeroRoll,

    EnemyVoiceRegular,
    EnemyVoiceTiny,
    EnemyVoiceBoss,
    EnemyAttackPrepare,
    EnemyAttack,
    EnemyPoofSmall,
    EnemyPoofBig,

    HitPaf,
    HitLightning,
    HitFire,
    HitSlash,

    CardChangeSelection,
    ClicChangeSelection,

    MenuClic,
    WaveStart,
    WaveEnd,

    Guitar,
    
    HeroAttackThrowSmall,
    EnemyVoicSmall,
    EnemyVoiceBig,
    OpenChest,

    BossVoice,
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