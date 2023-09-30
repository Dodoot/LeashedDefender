using System;
using UnityEngine;

public class Doggo : MonoBehaviour
{
    private const string ANIMATOR_TRIGGER_BARK = "Bark";

    [Header("Leash")]
    [SerializeField] private float _leashRadius = 3f;
    [SerializeField] private float _reboundRadiusThreshold = .5f;
    [SerializeField] private float _leashForceMultiplier = 5f;
    [SerializeField] private float _leashReboundIntensity = 50f;
    [SerializeField] private float _reboundTime = 1f;
    [SerializeField] private float _reboundMaxSpeed = 1f;

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

    private Vector2 _previousInputMove;
    private Vector2 _inputMove;
    private float _leashTension;
    private float _reboundTimer;

    private float MaxSpeed => _reboundTimer > 0 ? _reboundMaxSpeed : _defaultMaxSpeed;

    void Update()
    {
        UpdateInputs();
        UpdateTension();
        
        Bark();
    }

    void FixedUpdate()
    {
        UpdateForces();
        UpdateDrag();

        ClampSpeed();
        
        MoveHuman();
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
    }

    #endregion

    #region Movement
    private void UpdateDrag()
    {
        float drag;

        if (_reboundTimer > 0)
        {
            drag = 0;

            _reboundTimer -= Time.deltaTime;
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

    private void UpdateForces()
    {
        _rigidBody.AddForce(_inputMove * _moveForce);

        if (_leashTension >= 0)
        {
            Vector2 leashForce;

            if (_leashTension >= _reboundRadiusThreshold && _previousInputMove.sqrMagnitude > 0 && _inputMove.magnitude <= _inputDeadZone)
            {
                leashForce = GameManager.LeashDirection * _leashReboundIntensity;

                _reboundTimer = _reboundTime;
    
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
}
