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
        //Main.Object.Spawn<Boom>(1, transform.position); 폭발이펙트 생성 및 해당 이펙트와 닿는 적 데미지 판정
        Main.Object.Despawn(this);
    }
}