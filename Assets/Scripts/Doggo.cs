using UnityEngine;

public class Doggo : MonoBehaviour
{
    [Header("Leash")]
    [SerializeField] private float _leashRadius = 3f;
    [SerializeField] private float _leashForceMultiplier = 5f;
    [SerializeField] private float _leashReboundIntensity = 50f;
    [SerializeField] private float _reboundTime = 1f;

    [Header("Movement")]
    [SerializeField] private float _moveForce = 1f;
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _inputDeadZone = .1f;
    [SerializeField] private float _breakDrag = 10f;
    [SerializeField] private float _defaultDrag = 3f;
    [SerializeField] private float _stopSpeedThreshold = 1f;

    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;

    private Vector2 _previousInputMove;
    private Vector2 _inputMove;
    private float _leashTension;
    private float _reboundTimer;

    void Update()
    {
        UpdateInputs();
        UpdateForces();
    }

    void FixedUpdate()
    {
        UpdateDrag();

        if (_rigidBody.velocity.magnitude > _maxSpeed)
        {
            _rigidBody.velocity = _rigidBody.velocity.normalized * _maxSpeed;
        }
        if (_rigidBody.velocity.magnitude < _stopSpeedThreshold)
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

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
    }

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

        _leashTension = (transform.position - GameManager.Human.transform.position).sqrMagnitude - _leashRadius * _leashRadius;

        if (_leashTension >= 0)
        {
            var leashDirection = (GameManager.Human.transform.position - transform.position).normalized;
            Vector2 leashForce;

            if (_previousInputMove.sqrMagnitude > 0 && _inputMove.magnitude <= _inputDeadZone)
            {
                leashForce = leashDirection * _leashReboundIntensity;

                _reboundTimer = _reboundTime;
            }
            else
            {
                leashForce = leashDirection * _leashTension * _leashForceMultiplier;
            }

            _rigidBody.AddForce(leashForce);
        }
    }
}
