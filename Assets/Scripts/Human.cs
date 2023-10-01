using System;
using UnityEngine;

public class Human : MonoBehaviour
{
    private const string ANIMATOR_BOOL_PULL = "Pull";
    private const string ANIMATOR_BOOL_HURT = "Hurt";

    [SerializeField] private int _startLife = 3;
    [SerializeField] private float _invincibilityTime = 1f;

    [Header("Movement")]
    [SerializeField] private float leashTensionThreshold = .5f;
    [SerializeField] private float leashMoveForceSpeedMultiplier = .2f;

    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _leashPoint = null;

    private int _currentLife;
    private float _invincibilityTimer;

    public Transform LeashPoint => _leashPoint;
    public int CurrentLife => _currentLife;

    private void Start()
    {
        _currentLife = _startLife;
        HUDManager.Refresh();
    }

    private void Update()
    {
        UpdateInvincibility();
    }

    private void UpdateInvincibility()
    {
        if (_invincibilityTimer > 0)
        {
            _invincibilityTimer -= Time.deltaTime;
        }

        _animator.SetBool(ANIMATOR_BOOL_HURT, _invincibilityTimer > 0);
    }

    public void Move(float leashTension)
    {
        if (leashTension >= 0)
        {
            _rigidBody.velocity = leashMoveForceSpeedMultiplier * leashTension * -GameManager.LeashDirection;

            transform.localScale = new Vector3(GameManager.LeashDirection.x < 0 ? 1f : -1f, 1f, 1f);
        }
        else
        {
            _rigidBody.velocity = Vector2.zero;
        }

        _animator.SetBool(ANIMATOR_BOOL_PULL, leashTension > 0);
    }

    public void Hurt()
    {
        if (_invincibilityTimer <= 0)
        {
            FXManager.TriggerHurtFX();
            MusicAndSoundManager.PlaySound(ESoundName.Oof);

            _currentLife--;
            HUDManager.Refresh();

            _invincibilityTimer = _invincibilityTime;

            if (_currentLife <= 0)
            {
                GameManager.Lose();
            }
        }
    }
}
