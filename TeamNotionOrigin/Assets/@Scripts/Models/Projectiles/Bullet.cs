using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : Projectile
{
    [SerializeField] private LayerMask _layer;

    public float speed = 10f;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        Main.Object.Despawn(this);
    }
}
