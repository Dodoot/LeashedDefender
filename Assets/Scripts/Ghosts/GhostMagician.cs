using UnityEngine;

public class GhostMagician : Ghost
{
    [SerializeField] private float _rotationMaxAngle = 50f;
    [SerializeField] private float _sinusTime = 3f;

    [SerializeField] private float _maxY = 4.5f;
    [SerializeField] private float _targetDistance = 5f;
    [SerializeField] private float _maxDistance = 6f;
    [SerializeField] private float _attackCooldown = 3f;

    [SerializeField] private GameObject _projectilePrefab = null;
    [SerializeField] private Transform _projectileSpawnPoint = null;

    private float _angleTimer;
    private bool _isAttacking;
    private float _attackTimer; 

    protected override void Update()
    {
        if (!_isDead)
        {
            CheckDistance();
        
            if (_barkedTimer > 0)
            {
                _barkedTimer -= Time.deltaTime;

                _rigidBody.velocity = _barkedDirection * _barkedSpeed;
            }
            else
            {
                if (!_isAttacking)
                {
                    Move();
                    FaceCorrectDirection();
                }
                else
                {
                    _rigidBody.velocity = Vector2.zero;
                    UpdateAttack();
                }
            }
        }
    }

    private void CheckDistance()
    {
        if(!_isAttacking 
           && (GameManager.Human.transform.position - transform.position).magnitude <= _targetDistance
           && Mathf.Abs(transform.position.y) < _maxY)
        {
            _isAttacking = true;
            _rigidBody.velocity = Vector3.zero;
        }
        if (_isAttacking && (GameManager.Human.transform.position - transform.position).magnitude > _maxDistance)
        {
            _animator.SetTrigger("StopAttack");
            _isAttacking = false;
        }
    }

    protected override void Move()
    {
        _angleTimer = (_angleTimer + Time.deltaTime) % _sinusTime;

        var direction = (GameManager.Human.transform.position - transform.position).normalized;

        direction = Quaternion.AngleAxis(Mathf.Sin(_angleTimer / _sinusTime * 2 * Mathf.PI) * _rotationMaxAngle, Vector3.forward) * direction;

        _rigidBody.velocity = direction * _speed;
    }

    private void UpdateAttack()
    {
        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0)
        {
            _animator.SetTrigger("Attack");

            _attackTimer = _attackCooldown;
        }
    }

    public void ThrowFireball()
    {
        var fireball = Instantiate(_projectilePrefab, _projectileSpawnPoint.position, Quaternion.identity);
        fireball.transform.up = (GameManager.Human.transform.position - transform.position).normalized;
    }
}
