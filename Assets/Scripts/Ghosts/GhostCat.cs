using UnityEngine;

public class GhostCat : Ghost
{
    [SerializeField] private float _rotationAngle = 60f;

    private float _trueRotationAngle;

    private void Start()
    {
        _trueRotationAngle = Random.value > .5f ? _rotationAngle : -_rotationAngle;
    }

    protected override void Move()
    {
        var direction = (GameManager.Human.transform.position - transform.position).normalized;

        direction = Quaternion.AngleAxis(_trueRotationAngle, Vector3.forward) * direction;

        _rigidBody.velocity = direction * _speed;
    }
}
