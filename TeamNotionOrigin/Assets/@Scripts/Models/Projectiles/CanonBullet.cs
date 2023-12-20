using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBullet : Projectile
{
    [SerializeField] private LayerMask _layer;
    public float speed = 10f;

    protected override void OnTriggerEnter2D(Collider2D collision)
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
        base.OnTriggerEnter2D(collision);
        //Main.Object.Spawn<Boom>(1, transform.position); 폭발이펙트 생성 및 해당 이펙트와 닿는 적 데미지 판정
        Main.Object.Despawn(this);
    }
}