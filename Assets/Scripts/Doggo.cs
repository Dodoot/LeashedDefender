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
}

public class Doggo : MonoBehaviour
{
    private const string ANIMATOR_TRIGGER_BARK = "Bark";
    private const string ANIMATOR_BOOL_DASH = "Dash";

    [Header("Fences")]
    [SerializeField] private float _fenceX = 7.2f;
    [SerializeField] private float _fenceY = 4.7f;

    [Header("Leash")]
    [SerializeField] private float _leashRadius = 3f;
    [SerializeField] private float _leashForceMultiplier = 5f;

    [Header("Dash")]
    [SerializeField] private ChargeLevel[] _chargeLevels = null; // Needs to be ordered
    [SerializeField] private float _dashMaxSpeed = 1f;

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

    private void ClampPosition()
    {
        if (Mathf.Abs(transform.position.x) > _fenceX || Mathf.Abs(transform.position.y) > _fenceY)
        {
            var newPos = new Vector2(Mathf.Clamp(transform.position.x, -_fenceX, _fenceX), Mathf.Clamp(transform.position.y, -_fenceY, _fenceY));
            transform.position = newPos;
        }
    }

    private void FaceCorrectDirection()
    {
        if(_inputMove.SqrMagnitude() > _inputDeadZone)
        {
            transform.localScale = new Vector3(_inputMove.x < 0 ? 1f : -1f, 1f, 1f);
        }
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
            }
            else
            {
                _chargeTimer = 0;
                ScreenShakeManager.SetGlobalShake(0);
            }
        }

        _animator.SetBool(ANIMATOR_BOOL_DASH, _dashTimer > 0 && _shouldAttack);
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

    private void UpdateTension()
    {
        _leashTension = (transform.position - GameManager.Human.transform.position).sqrMagnitude - _leashRadius * _leashRadius;
    }

    private void MoveHuman()
    {
        GameManager.Human.Move(_leashTension);
    }

    private void Bark()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger(ANIMATOR_TRIGGER_BARK);
        }
    }

    #region Inputs

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

    #endregion

    #region Movement
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
            Vector2 leashForce;
            
            if (_dashTrigger)
            {
                var chargeLevel = GetChargeLevel();

                leashForce = GameManager.LeashDirection * chargeLevel.Force;
                _dashTimer = chargeLevel.Time;
                _shouldAttack = chargeLevel.ActivateAttack;

                _chargeTimer = 0;
                ScreenShakeManager.SetGlobalShake(0);
                _dashTrigger = false;

                _rigidBody.AddForce(leashForce, ForceMode2D.Impulse);
            }
            else
            {
                leashForce = GameManager.LeashDirection * _leashTension * _leashForceMultiplier;

                _rigidBody.AddForce(leashForce);
            }
        }
    }

    #endregion

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
