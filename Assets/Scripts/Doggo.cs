using UnityEngine;

public class Doggo : MonoBehaviour
{
    private const string ANIMATOR_TRIGGER_BARK = "Bark";
    private const string ANIMATOR_BOOL_DASH = "Dash";

    [Header("Leash")]
    [SerializeField] private float _leashRadius = 3f;
    [SerializeField] private float _leashForceMultiplier = 5f;
    [SerializeField] private float _dashRadiusThreshold = 4f;
    [SerializeField] private float _dashIntensity = 50f;
    [SerializeField] private float _dashTime = 1f;
    [SerializeField] private float _dashMaxSpeed = 1f;
    [SerializeField] private float _dashChargeMaxTime = 3f;

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
    private float _dashCharge;
    private bool _dashTrigger;

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

        MoveHuman();
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
                _dashCharge += Time.fixedDeltaTime;
                _dashCharge = Mathf.Min(_dashCharge, _dashChargeMaxTime);

                ScreenShakeManager.SetGlobalShake(_dashCharge / _dashChargeMaxTime / 10);
            }
            else
            {
                _dashCharge = 0;
                ScreenShakeManager.SetGlobalShake(0);
            }
        }

        _animator.SetBool(ANIMATOR_BOOL_DASH, _dashTimer > 0);
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
                leashForce = GameManager.LeashDirection * _dashIntensity;

                var chargeT = _dashCharge / _dashChargeMaxTime;
                _dashTimer = _dashTime * Mathf.Lerp(0f, 1.5f, chargeT);

                _dashCharge = 0;
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
}
