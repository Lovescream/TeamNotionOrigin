using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _layer;

    public float speed = 10f;
    private float _currentDuration;
    private Vector2 _direction;
    private bool _isReady;

    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Projectile _projectile;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(!_isReady)
        {
            return;
        }

        _currentDuration += Time.deltaTime;

        if(_currentDuration > 5f)
        {
            Main.Object.Despawn(_projectile);
        }

        _rigidbody.velocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layer.value == (_layer.value | (1<<collision.gameObject.layer)))
        {
            Main.Object.Despawn(_projectile);
        }
    }

    public void InitializeAttack(Vector2 direction, Projectile projectile)
    {
        _projectile = projectile;
        _direction = direction;

        UpdateProjectilSprite();
        _currentDuration = 0;
        
        _isReady = true;
    }

    private void UpdateProjectilSprite()
    {
        transform.localScale = Vector3.one;
    }
}
