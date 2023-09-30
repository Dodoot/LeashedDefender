using UnityEngine;

public class Doggo : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveForce = 1f;
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private float _breakThreshold = .1f;
    [SerializeField] private float _breakDrag = 10f;
    [SerializeField] private float _defaultDrag = 3f;
    [SerializeField] private float _stopSpeedThreshold = 1f;

    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;

    private Vector2 _inputMove;

    void Update()
    {
        UpdateInputs();
        Move();
    }

    void FixedUpdate()
    {
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

    private void Move()
    {
        _rigidBody.AddForce(_inputMove * _moveForce);

        _rigidBody.drag = _inputMove.magnitude <= _breakThreshold ? _breakDrag : _defaultDrag;
    }
}
