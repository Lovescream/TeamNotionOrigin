using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : Projectile
{
    private float lifetime = 1.0f;

    private void Update()
    {
        lifetime -=Time.deltaTime;
    }
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<Creature>(out var creature))
        {
            creature.Hp -= Damage/2;
        }
        if (lifetime <= 0)
            Main.Object.Despawn(this);
    }
}