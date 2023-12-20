using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : Projectile
{
    [SerializeField] private LayerMask _layer;
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private Vector3 _randDirection;
    public float speed = 10f;
    float randomSpread;
    private bool _initialize;
    private void Awake()
    {
        Camera camera = Camera.main;
        mousePoint = camera.ScreenToWorldPoint(Input.mousePosition);
        randomSpread = Random.Range(-0.5f, 0.5f);
        _randDirection = Quaternion.Euler(0, 0, randomSpread) * mousePoint.normalized;
    }

    protected override void FixedUpdate()
    {
        transform.position += _randDirection;
    }

    public override bool Initialize()
    {
        if (!_initialize) return false;
        _initialize = true;
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
        return true;
    }

}