using System;
using UnityEngine;

[Serializable]
public struct ChargeLevel
{
    public float Threshold;
    public bool ActivateAttack;
    public float Force;
    public float Time;
    public float ScreenShake;
    public Color LeashColor;
    public float AudioVolume;
}

public class Doggo : MonoBehaviour
{
    private const string ANIMATOR_TRIGGER_BARK = "Bark";
    private const string ANIMATOR_BOOL_DASH = "Dash";
    private const string ANIMATOR_BOOL_WALK = "Walk";

    [Header("Fences")]
    [SerializeField] private float _fenceX = 7.2f;
    [SerializeField] private float _fenceY = 4.7f;

    [Header("Leash")]
    [SerializeField] private float _leashRadius = 3f;
    [SerializeField] private float _leashForceMultiplier = 5f;

    [Header("Dash")]
    [SerializeField] private ChargeLevel[] _chargeLevels = null; // Needs to be ordered
    [SerializeField] private float _dashMaxSpeed = 1f;
    [SerializeField] private GameObject _dashObject = null;

    [Header("Movement")]
    [SerializeField] private float _moveForce = 1f;
    [SerializeField] private float _defaultMaxSpeed = 1f;
    [SerializeField] private float _inputDeadZone = .1f;
    [SerializeField] private float _breakDrag = 10f;
    [SerializeField] private float _defaultDrag = 3f;
    [SerializeField] private float _stopSpeedThreshold = 1f;

    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _leashPoint = null;

    [Header("Audio")]
    [SerializeField] private AudioSource _walkAudio = null;
    [SerializeField] private float _walkAudioVolume = .5f;
    [SerializeField] private AudioSource _leashAudio = null;

    private Vector2 _previousInputMove;
    private Vector2 _inputMove;
    private float _leashTension;
    private float _dashTimer;
    private float _chargeTimer;
    private bool _dashTrigger;
    private bool _shouldAttack;

    private float MaxSpeed => _dashTimer > 0 ? _dashMaxSpeed : _defaultMaxSpeed;

    public Transform LeashPoint => _leashPoint;

    void Update()
    {
        UpdateInputs();

        _animator.SetBool(ANIMATOR_BOOL_WALK, _inputMove.SqrMagnitude() > _inputDeadZone);
        _walkAudio.volume = _inputMove.SqrMagnitude() > _inputDeadZone && _dashTimer <= 0 ? _walkAudioVolume : 0f;

        FaceCorrectDirection();
        Bark();
    }

    void FixedUpdate()
    {
        UpdateTension();
        UpdateDash();
        
        ApplyForces();
        UpdateDrag();
        ClampSpeed();
        ClampPosition();

        MoveHuman();
    }


    #region Update

    private void UpdateInputs()
    {
        _previousInputMove = _inputMove;

        _inputMove = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            _inputMove += Vector2.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _inputMove += Vector2.down;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _inputMove += Vector2.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            _inputMove += Vector2.right;
        }

        _inputMove = Vector2.ClampMagnitude(_inputMove, 1f);


        if (_previousInputMove.sqrMagnitude > 0 && _inputMove.magnitude <= _inputDeadZone && _leashTension > 0)
        {
            _dashTrigger = true;
        }
    }

    private void FaceCorrectDirection()
    {
        if (_dashTimer > 0 && _shouldAttack)
        {
            transform.localScale = new Vector3(_rigidBody.velocity.x < 0 ? 1f : -1f, 1f, 1f);
        }

        if (_inputMove.SqrMagnitude() > _inputDeadZone)
        {
            transform.localScale = new Vector3(_inputMove.x < 0 ? 1f : -1f, 1f, 1f);
        }
    }

    private void Bark()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger(ANIMATOR_TRIGGER_BARK);
            MusicAndSoundManager.PlaySound(ESoundName.Waf);
        }
    }

    #endregion

    #region Fixed Update

    private void UpdateTension()
    {
        _leashTension = (transform.position - GameManager.Human.transform.position).sqrMagnitude - _leashRadius * _leashRadius;
    }

    private void UpdateDash()
    {
        if (_dashTimer > 0)
        {
            _dashTimer -= Time.fixedDeltaTime;
        }
        else if (_inputMove.SqrMagnitude() > _inputDeadZone)
        {
            if (_leashTension > 0)
            {
                _chargeTimer += Time.fixedDeltaTime;

                ScreenShakeManager.SetGlobalShake(GetChargeLevel().ScreenShake);
                _leashAudio.volume = GetChargeLevel().AudioVolume;
            }
            else
            {
                _chargeTimer = 0;
                ScreenShakeManager.SetGlobalShake(0);
                _leashAudio.volume = 0;
            }
        }

        _animator.SetBool(ANIMATOR_BOOL_DASH, _dashTimer > 0 && _shouldAttack);
    }

    private void UpdateDrag()
    {
        float drag;

        if (_dashTimer > 0)
        {
            drag = 0;
        }
        else if (_inputMove.magnitude <= _inputDeadZone || _leashTension > 0)
        {
            drag = _breakDrag;
        }
        else
        {
            drag = _defaultDrag;
        }

        _rigidBody.drag = drag;
    }

    private void ApplyForces()
    {
        _rigidBody.AddForce(_inputMove * _moveForce);

        if (_leashTension > 0)
        {
            if (_dashTrigger)
            {
                LaunchDash();
            }
            else
            {
                var leashForce = GameManager.LeashDirection * _leashTension * _leashForceMultiplier;

                _rigidBody.AddForce(leashForce);
            }
        }
    }

    private void ClampSpeed()
    {
        if (_rigidBody.velocity.magnitude > MaxSpeed)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * MaxSpeed;
        }
        if (_rigidBody.velocity.magnitude < _stopSpeedThreshold)
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

    private void ClampPosition()
    {
        if (Mathf.Abs(transform.position.x) > _fenceX || Mathf.Abs(transform.position.y) > _fenceY)
        {
            var newPos = new Vector2(Mathf.Clamp(transform.position.x, -_fenceX, _fenceX), Mathf.Clamp(transform.position.y, -_fenceY, _fenceY));
            transform.position = newPos;
        }
    }

    private void MoveHuman()
    {
        GameManager.Human.Move(_leashTension);
    }

    #endregion

    private void LaunchDash()
    {
        var chargeLevel = GetChargeLevel();

        var leashForce = GameManager.LeashDirection * chargeLevel.Force;
        _dashTimer = chargeLevel.Time;
        _shouldAttack = chargeLevel.ActivateAttack;

        _dashObject.transform.localEulerAngles = new Vector3(0f, 0f, 
            Vector2.Angle(GameManager.LeashDirection.x < 0 ? Vector2.left : Vector2.right, GameManager.LeashDirection)
            * (GameManager.LeashDirection.y > 0 ? -1f : 1f));

        _chargeTimer = 0;
        ScreenShakeManager.SetGlobalShake(0);
        _leashAudio.volume = 0;
        ScreenShakeManager.SetTempShake(chargeLevel.ScreenShake * 2, .1f);
        _dashTrigger = false;

        _rigidBody.AddForce(leashForce, ForceMode2D.Impulse);

        if (_shouldAttack)
        {
            MusicAndSoundManager.PlaySound(ESoundName.Dash);
        }
        MusicAndSoundManager.PlaySound(ESoundName.Waf);
    }

    public ChargeLevel GetChargeLevel()
    {
        var result = new ChargeLevel();

        foreach (var chargeLevel in _chargeLevels)
        {
            if (_chargeTimer >= chargeLevel.Threshold)
            {
                result = chargeLevel;
            }
        }

        return result;
    }
}
