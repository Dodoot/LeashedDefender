using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : Singleton<FXManager>
{
    private const string ANIMATOR_TRIGGER_HURT = "Hurt";

    [SerializeField] private Animator _animator;

    public static void TriggerHurtFX()
    {
        _instance._animator.SetTrigger(ANIMATOR_TRIGGER_HURT);
    }
}
