using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBullet : Projectile
{
    [SerializeField] private LayerMask _layer;
    public float speed = 10f;
    protected override void FixedUpdate()
    {
        transform.position += mousePoint.normalized;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Creature>(out var creature))
        {
            creature.Hp -= Damage;
            Main.Object.Spawn<Boom>(1, transform.position);
            Main.Object.Despawn(this);
        }
        if (collision.gameObject.layer == 0)
        {
            Main.Object.Spawn<Boom>(1, transform.position);
            Main.Object.Despawn(this);
        }

    }
}
