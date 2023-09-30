using UnityEngine;

public class Human : MonoBehaviour
{
    private const string ANIMATOR_BOOL_PULL = "Pull";

    [Header("Movement")]
    [SerializeField] private float leashTensionThreshold = .5f;
    [SerializeField] private float leashMoveForceSpeedMultiplier = .2f;

    [Header("Inner References")]
    [SerializeField] private Rigidbody2D _rigidBody = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private Transform _leashPoint = null;

    public Transform LeashPoint => _leashPoint;

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
}
