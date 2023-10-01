using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected Rigidbody2D _rigidBody = null;
    [SerializeField] private float _barkedTime = .3f;
    [SerializeField] protected float _barkedSpeed = 5f;
    [SerializeField] private ESoundName _soundName = ESoundName.None;
    [SerializeField] protected Animator _animator = null;

    protected float _barkedTimer;
    protected Vector2 _barkedDirection;
    protected bool _isDead;

    protected virtual void Update()
    {
        if (!_isDead)
        {
            if (_barkedTimer > 0)
            {
                _barkedTimer -= Time.deltaTime;

                _rigidBody.velocity = _barkedDirection * _barkedSpeed;
            }
            else
            {
                Move();
                FaceCorrectDirection();
            }
        }
    }

    protected abstract void Move();

    protected void FaceCorrectDirection()
    {
        if (_rigidBody.velocity.SqrMagnitude() > 0)
        {
            transform.localScale = new Vector3(_rigidBody.velocity.x > 0 ? 1f : -1f, 1f, 1f);
        }
    }

    public virtual void Die()
    {
        if (!_isDead)
        {
            _isDead = true;
            _rigidBody.velocity = Vector2.zero;
            _animator.SetTrigger("Die");
            MusicAndSoundManager.PlaySound(_soundName);
        }
    }

    public void Barked(Vector2 direction)
    {
        _barkedTimer = _barkedTime;
        _barkedDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isDead)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("HumanHurtBox"))
            {
                GameManager.Human.Hurt();
                Destroy(gameObject);
            }
        }
    }

    public void AnimatorCallDestroy()
    {
        Destroy(gameObject);
    }
}
