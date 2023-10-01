public class GhostBasic : Ghost
{
    protected override void Move()
    {
        var direction = (GameManager.Human.transform.position - transform.position).normalized;

        _rigidBody.velocity = direction * _speed;
    }
}
