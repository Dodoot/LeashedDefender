using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected Rigidbody2D _rigidBody = null;

    protected virtual void Update()
    {
        Move();
    }

    protected abstract void Move();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
