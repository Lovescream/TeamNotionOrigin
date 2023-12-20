using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBullet : Projectile
{
    [SerializeField] private LayerMask _layer;
    public float speed = 10f;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Main.Object.Spawn<Boom>(1, transform.position);
        Main.Object.Despawn(this);
    }
}