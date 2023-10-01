using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private Rigidbody2D _rigidBody = null;
    [SerializeField] private Animator _animator = null;

    [SerializeField] private float _maxX = 8f;
    [SerializeField] private float _maxY = 5.5f;

    private bool _bounced;

    private void Start()
    {
        _rigidBody.velocity = _speed * transform.up;
        MusicAndSoundManager.PlaySound(ESoundName.Fireball);
    }

    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > _maxX || Mathf.Abs(transform.position.y) > _maxY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BarkHitbox"))
        {
            if (!_bounced)
            {
                transform.up = -transform.up;
                _rigidBody.velocity = _speed * transform.up;

                _bounced = true;
                _animator.SetTrigger("Fade");
            }
        }
        else
        {
            GameManager.Human.Hurt();
            Destroy(gameObject);
        }
    }
}
