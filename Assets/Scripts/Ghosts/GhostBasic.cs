using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBasic : Ghost
{
    protected override void Move()
    {
        var direction = (GameManager.Human.transform.position - transform.position).normalized;

        _rigidBody.velocity = direction * _speed;
    }
}
