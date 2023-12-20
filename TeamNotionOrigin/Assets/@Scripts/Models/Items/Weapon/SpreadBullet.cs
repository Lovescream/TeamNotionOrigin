using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadBullet : Projectile
{
    [SerializeField] private LayerMask _layer;
    private Vector3 _randDirection;
    public float speed = 10f;
    private void Awake()
    {
        float randomSpread = Random.Range(-0.5f, 0.5f);
        _randDirection = new Vector3(randomSpread, randomSpread, 0);
    }

    protected override void FixedUpdate()
    {
        transform.position += mousePoint.normalized + _randDirection;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layer.value == (_layer.value | (1 << collision.gameObject.layer)))
        {
            Main.Object.Despawn(this);
        }
    }

}