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
        transform.position += mousePoint.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 0)
        {
            Main.Object.Despawn(this);
        }
    }
}
