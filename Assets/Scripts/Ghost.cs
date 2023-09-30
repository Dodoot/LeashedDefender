using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private Rigidbody2D _rigidBody = null;

    void Update()
    {
        var direction = (GameManager.Human.transform.position - transform.position).normalized;

        _rigidBody.velocity = direction * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
