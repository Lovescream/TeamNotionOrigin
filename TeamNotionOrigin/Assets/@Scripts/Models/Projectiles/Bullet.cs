using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : Projectile
{
    [SerializeField] private LayerMask _layer;

    public float speed = 10f;
    protected override void FixedUpdate()
    {
        transform.position += mousePoint.normalized; //현재 마우스 위치 읽어와서 방향 설정
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_layer.value == (_layer.value | (1<<collision.gameObject.layer)))
        {
            Main.Object.Despawn(this);
        }
    }
}
