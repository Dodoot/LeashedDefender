using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ghost : MonoBehaviour
{
    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected Rigidbody2D _rigidBody = null;
    [SerializeField] private float _barkedTime = .3f;
    [SerializeField] protected float _barkedSpeed = 5f;
    [SerializeField] private ESoundName _soundName = ESoundName.None;

    protected float _barkedTimer;
    protected Vector2 _barkedDirection;

    protected virtual void Update()
    {
        if (_barkedTimer > 0)
        {
            _barkedTimer -= Time.deltaTime;

            _rigidBody.velocity = _barkedDirection * _barkedSpeed;
        }
        else
        {
            Move();
        }
    }

    protected abstract void Move();

    public virtual void Die()
    {
        Destroy(gameObject);
        MusicAndSoundManager.PlaySound(_soundName);
    }

    public void Barked(Vector2 direction)
    {
        _barkedTimer = _barkedTime;
        _barkedDirection = direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("HumanHurtBox"))
        {
            GameManager.Human.Hurt();
            Destroy(gameObject);
        }
    }
}
